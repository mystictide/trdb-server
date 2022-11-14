using Dapper.Contrib.Extensions;
using Dapper;
using trdb.data.Interface.Films.Junctions;
using trdb.data.Repo.Helpers;
using trdb.entity.Helpers;
using trdb.entity.Films.Junctions;

namespace trdb.data.Repo.Films.Junctions
{
    public class FilmGenreJunctionRepository : Connection.dbConnection, IFilmGenreJunction
    {
        public async Task<FilmGenreJunction> Add(FilmGenreJunction entity)
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
                    await con.DeleteAsync(new FilmGenreJunction() { ID = ID });
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

        public async Task<FilteredList<FilmGenreJunction>> FilteredList(FilteredList<FilmGenreJunction> request)
        {
            try
            {
                FilteredList<FilmGenreJunction> result = new FilteredList<FilmGenreJunction>();
                DynamicParameters param = new DynamicParameters();
                param.Add("@Keyword", request.filter.Keyword);
                param.Add("@PageSize", request.filter.pageSize);

                string WhereClause = @" WHERE (t.Name like '%' + @Keyword + '%')";

                string query_count = $@"  Select Count(t.ID) from FilmGenreJunction t {WhereClause}";

                string query = $@"
                SELECT *
                FROM FilmGenreJunction t
                {WhereClause} 
                ORDER BY t.ID ASC 
                OFFSET @StartIndex ROWS
                FETCH NEXT @PageSize ROWS ONLY";

                using (var con = GetConnection)
                {
                    result.totalItems = await con.QueryFirstOrDefaultAsync<int>(query_count, param);
                    request.filter.pager = new Page(result.totalItems, request.filter.pageSize, request.filter.page);
                    param.Add("@StartIndex", request.filter.pager.StartIndex);
                    result.data = await con.QueryAsync<FilmGenreJunction>(query, param);
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

        public async Task<FilmGenreJunction> Get(int ID)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@ID", ID);

                string query = $@"
                SELECT *
                FROM FilmGenreJunction 
                WHERE ID = @ID";

                using (var con = GetConnection)
                {
                    var res = await con.QueryAsync<FilmGenreJunction>(query, param);
                    return res.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                LogsRepository.CreateLog(ex);
                return null;
            }
        }

        public async Task<IEnumerable<FilmGenreJunction>> GetAll()
        {
            try
            {
                using (var con = GetConnection)
                {
                    var res = await con.GetAllAsync<FilmGenreJunction>();
                    return res;
                }
            }
            catch (Exception ex)
            {
                LogsRepository.CreateLog(ex);
                return null;
            }
        }

        public async Task<List<entity.Films.FilmGenres>> Manage(List<entity.Films.FilmGenres> entity, int FilmID)
        {
            var result = new List<entity.Films.FilmGenres>();
            try
            {
                using (var con = GetConnection)
                {
                    foreach (var item in entity)
                    {
                        DynamicParameters param = new DynamicParameters();
                        param.Add("@FilmID", FilmID);
                        param.Add("@GenreID", item.TMDB_ID);

                        string query = $@"
                        DECLARE  @result table(ID Int, FilmID Int, GenreID Int)
                        IF EXISTS(SELECT * from FilmGenreJunction where FilmID = @FilmID AND GenreID = @GenreID)        
                        BEGIN            
                        UPDATE FilmGenreJunction
                                    SET FilmID = @FilmID, GenreID = @GenreID
							        OUTPUT INSERTED.* INTO @result
                                    WHERE FilmID = @FilmID AND GenreID = @GenreID;
                        END                    
                        ELSE            
                        BEGIN  
                        INSERT INTO FilmGenreJunction (FilmID, GenreID)
                                     OUTPUT INSERTED.* INTO @result
                                     VALUES (@FilmID, @GenreID)
                        END
                        SELECT *
				        FROM @result";


                        var res = await con.QueryFirstOrDefaultAsync<FilmGenreJunction>(query, param);
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

        public async Task<ProcessResult> Update(FilmGenreJunction entity)
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