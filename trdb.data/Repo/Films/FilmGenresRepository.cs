using Dapper;
using Dapper.Contrib.Extensions;
using trdb.data.Interface.Films;
using trdb.data.Repo.Helpers;
using trdb.entity.Helpers;

namespace trdb.data.Repo.FilmGenres
{
    public class FilmGenresRepository : Connection.dbConnection, IFilmGenres
    {
        public async Task<entity.Films.FilmGenres> Add(entity.Films.FilmGenres entity)
        {
            ProcessResult result = new ProcessResult();
            try
            {
                using (var con = GetConnection)
                {
                    result.ReturnID = await con.InsertAsync(entity);
                    result.Message = "Genre saved successfully";
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
                    await con.DeleteAsync(new entity.Films.FilmGenres() { ID = ID });
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

        public async Task<FilteredList<entity.Films.FilmGenres>> FilteredList(FilteredList<entity.Films.FilmGenres> request)
        {
            try
            {
                FilteredList<entity.Films.FilmGenres> result = new FilteredList<entity.Films.FilmGenres>();
                DynamicParameters param = new DynamicParameters();
                param.Add("@Keyword", request.filter.Keyword);
                param.Add("@PageSize", request.filter.pageSize);

                string WhereClause = @" WHERE (t.Name like '%' + @Keyword + '%')";

                string query_count = $@"  Select Count(t.ID) from FilmGenres t {WhereClause}";

                string query = $@"
                SELECT *
                FROM FilmGenres t
                {WhereClause} 
                ORDER BY t.ID ASC 
                OFFSET @StartIndex ROWS
                FETCH NEXT @PageSize ROWS ONLY";

                using (var con = GetConnection)
                {
                    result.totalItems = await con.QueryFirstOrDefaultAsync<int>(query_count, param);
                    request.filter.pager = new Page(result.totalItems, request.filter.pageSize, request.filter.page);
                    param.Add("@StartIndex", request.filter.pager.StartIndex);
                    result.data = await con.QueryAsync<entity.Films.FilmGenres>(query, param);
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

        public async Task<entity.Films.FilmGenres> Get(int ID)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@ID", ID);

                string query = $@"
                SELECT *
                FROM FilmGenres 
                WHERE ID = @ID";

                using (var con = GetConnection)
                {
                    var res = await con.QueryAsync<entity.Films.FilmGenres>(query, param);
                    return res.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                LogsRepository.CreateLog(ex);
                return null;
            }
        }

        public async Task<IEnumerable<entity.Films.FilmGenres>> GetAll()
        {
            try
            {
                using (var con = GetConnection)
                {
                    var res = await con.GetAllAsync<entity.Films.FilmGenres>();
                    return res;
                }
            }
            catch (Exception ex)
            {
                LogsRepository.CreateLog(ex);
                return null;
            }
        }

        public async Task<List<entity.Films.FilmGenres>> GetFilmGenres(int FilmID)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@FilmID", FilmID);

                string query = $@"
                SELECT * FROM FilmGenres as g 
                WHERE g.TMDB_ID in
                (Select GenreID from FilmGenreJunction mg where mg.FilmID = @FilmID)
                ORDER BY TMDB_ID ASC";

                using (var con = GetConnection)
                {
                    var result = await con.QueryAsync<entity.Films.FilmGenres>(query, param);
                    return result.ToList();
                    ;
                }
            }
            catch (Exception ex)
            {
                LogsRepository.CreateLog(ex);
                return null;
            }
        }

        public async Task<List<entity.Films.FilmGenres>> Import(List<entity.Films.FilmGenres> entity)
        {
            var result = new List<entity.Films.FilmGenres>();
            try
            {
                using (var con = GetConnection)
                {
                    foreach (var item in entity)
                    {

                        DynamicParameters param = new DynamicParameters();
                        param.Add("@TMDB_ID", item.TMDB_ID);
                        param.Add("@Name", item.Name);

                        string query = $@"
                        DECLARE  @result table(ID Int, TMDB_ID Int, Name nvarchar(100))
                        IF EXISTS(SELECT * from FilmGenres where TMDB_ID = @TMDB_ID)        
                        BEGIN            
                        UPDATE FilmGenres
                                    SET TMDB_ID = @TMDB_ID, Name = @Name
							        OUTPUT INSERTED.* INTO @result
                                    WHERE TMDB_ID = @TMDB_ID;
                        END                    
                        ELSE            
                        BEGIN  
                        INSERT INTO FilmGenres (TMDB_ID, Name)
                                     OUTPUT INSERTED.* INTO @result
                                     VALUES (@TMDB_ID, @Name)
                        END
                        SELECT *
				        FROM @result";

                        var res = await con.QueryFirstOrDefaultAsync<entity.Films.FilmGenres>(query, param);
                        result.Add(res);
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

        public async Task<ProcessResult> Update(entity.Films.FilmGenres entity)
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
                        result.Message = "Genre updated successfully.";
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
