using Dapper.Contrib.Extensions;
using Dapper;
using trdb.data.Interface.Films.Junctions;
using trdb.data.Repo.Helpers;
using trdb.entity.Helpers;
using trdb.entity.Films.Junctions;
using trdb.entity.Films;

namespace trdb.data.Repo.Films.Junctions
{
    public class FilmLanguageJunctionRepository : Connection.dbConnection, IFilmLanguageJunction
    {
        public async Task<FilmLanguageJunction> Add(FilmLanguageJunction entity)
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
                    await con.DeleteAsync(new FilmLanguageJunction() { ID = ID });
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

        public async Task<FilteredList<FilmLanguageJunction>> FilteredList(FilteredList<FilmLanguageJunction> request)
        {
            try
            {
                FilteredList<FilmLanguageJunction> result = new FilteredList<FilmLanguageJunction>();
                DynamicParameters param = new DynamicParameters();
                param.Add("@Keyword", request.filter.Keyword);
                param.Add("@PageSize", request.filter.pageSize);

                string WhereClause = @" WHERE (t.Name like '%' + @Keyword + '%')";

                string query_count = $@"  Select Count(t.ID) from FilmLanguageJunction t {WhereClause}";

                string query = $@"
                SELECT *
                FROM FilmLanguageJunction t
                {WhereClause} 
                ORDER BY t.ID ASC 
                OFFSET @StartIndex ROWS
                FETCH NEXT @PageSize ROWS ONLY";

                using (var con = GetConnection)
                {
                    result.totalItems = await con.QueryFirstOrDefaultAsync<int>(query_count, param);
                    request.filter.pager = new Page(result.totalItems, request.filter.pageSize, request.filter.page);
                    param.Add("@StartIndex", request.filter.pager.StartIndex);
                    result.data = await con.QueryAsync<FilmLanguageJunction>(query, param);
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

        public async Task<FilmLanguageJunction> Get(int ID)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@ID", ID);

                string query = $@"
                SELECT *
                FROM FilmLanguageJunction 
                WHERE ID = @ID";

                using (var con = GetConnection)
                {
                    var res = await con.QueryAsync<FilmLanguageJunction>(query, param);
                    return res.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                LogsRepository.CreateLog(ex);
                return null;
            }
        }

        public async Task<IEnumerable<FilmLanguageJunction>> GetAll()
        {
            try
            {
                using (var con = GetConnection)
                {
                    var res = await con.GetAllAsync<FilmLanguageJunction>();
                    return res;
                }
            }
            catch (Exception ex)
            {
                LogsRepository.CreateLog(ex);
                return null;
            }
        }

        public async Task<List<Languages>> Manage(List<Languages> entity, int FilmID)
        {
            var result = new List<Languages>();
            try
            {
                using (var con = GetConnection)
                {
                    foreach (var item in entity)
                    {

                        DynamicParameters param = new DynamicParameters();
                        param.Add("@FilmID", FilmID);
                        param.Add("@LanguageID", item.ID);
                        param.Add("@Name", item.Name);

                        string query = $@"
                        DECLARE  @result table(ID Int, FilmID Int, LanguageID Int)
                        IF @LanguageID < 1
                        BEGIN
                        SET @LanguageID = (select ID from Languages where Name = @Name)
                        END
                        IF EXISTS(SELECT * from FilmLanguageJunction where FilmID = @FilmID AND LanguageID = @LanguageID)        
                        BEGIN            
                        UPDATE FilmLanguageJunction
                                    SET FilmID = @FilmID, LanguageID = @LanguageID
							        OUTPUT INSERTED.* INTO @result
                                    WHERE FilmID = @FilmID AND LanguageID = @LanguageID;
                        END                    
                        ELSE            
                        BEGIN  
                        INSERT INTO FilmLanguageJunction (FilmID, LanguageID)
                                     OUTPUT INSERTED.* INTO @result
                                     VALUES (@FilmID, @LanguageID)
                        END
                        SELECT *
				        FROM @result";

                        var res = await con.QueryFirstOrDefaultAsync<FilmLanguageJunction>(query, param);
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

        public async Task<ProcessResult> Update(FilmLanguageJunction entity)
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
