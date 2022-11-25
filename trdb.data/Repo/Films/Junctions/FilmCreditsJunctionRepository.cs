using Dapper.Contrib.Extensions;
using Dapper;
using trdb.data.Interface.Films.Junctions;
using trdb.data.Repo.Helpers;
using trdb.entity.Helpers;
using trdb.entity.Films.Junctions;
using trdb.entity.Films;

namespace trdb.data.Repo.Films.Junctions
{
    public class FilmCreditsJunctionRepository : Connection.dbConnection, IFilmCreditsJunction
    {
        public async Task<FilmCreditsJunction> Add(FilmCreditsJunction entity)
        {
            ProcessResult result = new ProcessResult();
            try
            {
                using (var con = GetConnection)
                {
                    result.ReturnID = await con.InsertAsync(entity);
                    result.Message = "Item saved successfully";
                    result.State = ProcessState.Success;
                    entity.ID = result.ReturnID;
                    return entity;
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.State = ProcessState.Error;
                return null;
            }
        }

        public async Task<ProcessResult> Delete(int ID)
        {
            using (var con = GetConnection)
            {
                ProcessResult pr = new ProcessResult();
                try
                {
                    await con.DeleteAsync(new FilmCreditsJunction() { ID = ID });
                    pr.ReturnID = 0;
                    pr.Message = "Success";
                    pr.State = ProcessState.Success;
                }
                catch (Exception)
                {
                    pr.ReturnID = 0;
                    pr.Message = "Error";
                    pr.State = ProcessState.Error;
                }
                return pr;
            }
        }

        public async Task<FilteredList<FilmCreditsJunction>> FilteredList(FilteredList<FilmCreditsJunction> request)
        {
            try
            {
                FilteredList<FilmCreditsJunction> result = new FilteredList<FilmCreditsJunction>();
                DynamicParameters param = new DynamicParameters();
                param.Add("@Keyword", request.filter.Keyword);
                param.Add("@PageSize", request.filter.pageSize);

                string WhereClause = @" WHERE (t.Name like '%' + @Keyword + '%')";

                string query_count = $@"  Select Count(t.ID) from FilmCreditsJunction t {WhereClause}";

                string query = $@"
                SELECT *
                FROM FilmCreditsJunction t
                {WhereClause} 
                ORDER BY t.ID ASC 
                OFFSET @StartIndex ROWS
                FETCH NEXT @PageSize ROWS ONLY";

                using (var con = GetConnection)
                {
                    result.totalItems = await con.QueryFirstOrDefaultAsync<int>(query_count, param);
                    request.filter.pager = new Page(result.totalItems, request.filter.pageSize, request.filter.page);
                    param.Add("@StartIndex", request.filter.pager.StartIndex);
                    result.data = await con.QueryAsync<FilmCreditsJunction>(query, param);
                    result.filter = request.filter;
                    result.filterModel = request.filterModel;
                    return result;
                }
            }
            catch (Exception ex)
            {
                LogsRepository.CreateLog(ex);
                return null;
            }
        }

        public async Task<FilmCreditsJunction> Get(int ID)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@ID", ID);

                string query = $@"
                SELECT *
                FROM FilmCreditsJunction 
                WHERE ID = @ID";

                using (var con = GetConnection)
                {
                    var res = await con.QueryAsync<FilmCreditsJunction>(query, param);
                    return res.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                LogsRepository.CreateLog(ex);
                return null;
            }
        }

        public async Task<IEnumerable<FilmCreditsJunction>> GetAll()
        {
            try
            {
                using (var con = GetConnection)
                {
                    var res = await con.GetAllAsync<FilmCreditsJunction>();
                    return res;
                }
            }
            catch (Exception ex)
            {
                LogsRepository.CreateLog(ex);
                return null;
            }
        }

        public async Task<List<People>> Manage(List<People> entity, int FilmID)
        {
            var result = new List<People>();
            try
            {
                using (var con = GetConnection)
                {
                    foreach (var item in entity)
                    {
                        DynamicParameters param = new DynamicParameters();
                        param.Add("@FilmID", FilmID);
                        param.Add("@PersonID", item.TMDB_ID);
                        param.Add("@Character", item.Character);
                        param.Add("@Department", item.Department);
                        param.Add("@Job", item.Job);
                        param.Add("@ListOrder", item.ListOrder);

                        string query = $@"
                        DECLARE  @result table(ID Int, FilmID Int, PersonID Int, Character nvarchar(MAX), Department nvarchar(250), Job nvarchar(250), ListOrder Int)
                        IF EXISTS(SELECT * from FilmCreditsJunction where FilmID = @FilmID AND PersonID = @PersonID AND Job = @Job)        
                        BEGIN            
                        UPDATE FilmCreditsJunction
                                    SET FilmID = @FilmID, PersonID = @PersonID, Character = @Character, Department = @Department, Job = @Job, ListOrder = @ListOrder
							        OUTPUT INSERTED.* INTO @result
                                    WHERE FilmID = @FilmID AND PersonID = @PersonID;
                        END                    
                        ELSE            
                        BEGIN  
                        INSERT INTO FilmCreditsJunction (FilmID, PersonID, Character, Department, Job, ListOrder)
                                     OUTPUT INSERTED.* INTO @result
                                     VALUES (@FilmID, @PersonID, @Character, @Department, @Job, @ListOrder)
                        END
                        SELECT *
				        FROM @result";

                        var res = await con.QueryFirstOrDefaultAsync<FilmCreditsJunction>(query, param);
                        item.ID = res.ID;
                        result.Add(item);
                    }
                    con.Dispose();
                }
            }
            catch (Exception ex)
            {
                LogsRepository.CreateLog(ex);
            }

            return result;
        }

        public async Task<ProcessResult> Update(FilmCreditsJunction entity)
        {
            ProcessResult result = new ProcessResult();
            try
            {
                using (var con = GetConnection)
                {
                    bool res = await con.UpdateAsync(entity);
                    if (res == true)
                    {
                        result.ReturnID = entity.ID;
                        result.Message = "Item updated successfully.";
                        result.State = ProcessState.Success;
                    }
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.State = ProcessState.Error;
            }
            return result;
        }
    }
}
