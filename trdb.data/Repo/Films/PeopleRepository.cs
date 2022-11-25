using Dapper;
using Dapper.Contrib.Extensions;
using trdb.data.Interface.Films;
using trdb.data.Repo.Helpers;
using trdb.entity.Helpers;
using trdb.entity.Films;

namespace trdb.data.Repo.Films
{
    public class PeopleRepository : Connection.dbConnection, IPeople
    {
        public async Task<People> Add(People entity)
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
                    await con.DeleteAsync(new People() { ID = ID });
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

        public async Task<FilteredList<People>> FilteredList(FilteredList<People> request)
        {
            try
            {
                FilteredList<People> result = new FilteredList<People>();
                DynamicParameters param = new DynamicParameters();
                param.Add("@Keyword", request.filter.Keyword);
                param.Add("@PageSize", request.filter.pageSize);
                param.Add("@Profession", request.filterModel.Profession);

                string WhereClause = @" WHERE t.Profession = @Profession AND (t.Name like '%' + @Keyword + '%') OR (t.Original_Name like '%' + @Keyword + '%')";

                string query_count = $@"  Select Count(t.ID) from People t {WhereClause}";

                string query = $@"
                SELECT *
                FROM People t
                {WhereClause} 
                ORDER BY t.ID ASC 
                OFFSET @StartIndex ROWS
                FETCH NEXT @PageSize ROWS ONLY";

                using (var con = GetConnection)
                {
                    result.totalItems = await con.QueryFirstOrDefaultAsync<int>(query_count, param);
                    request.filter.pager = new Page(result.totalItems, request.filter.pageSize, request.filter.page);
                    param.Add("@StartIndex", request.filter.pager.StartIndex);
                    result.data = await con.QueryAsync<People>(query, param);
                    result.filter = request.filter;
                    result.filterModel = request.filterModel;
                    result.filterModel.Pager = result.filter.pager;
                    return result;
                }
            }
            catch (Exception ex)
            {
                LogsRepository.CreateLog(ex);
                return null;
            }
        }

        public async Task<People> Get(int ID)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@ID", ID);

                string query = $@"
                SELECT *
                FROM People 
                WHERE ID = @ID";

                using (var con = GetConnection)
                {
                    var res = await con.QueryAsync<People>(query, param);
                    return res.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                LogsRepository.CreateLog(ex);
                return null;
            }
        }

        public async Task<IEnumerable<People>> GetAll()
        {
            try
            {
                using (var con = GetConnection)
                {
                    var res = await con.GetAllAsync<People>();
                    return res;
                }
            }
            catch (Exception ex)
            {
                LogsRepository.CreateLog(ex);
                return null;
            }
        }

        public async Task<List<People>> GetCast(int FilmID)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@FilmID", FilmID);

                string query = $@"
                SELECT *
                ,(select STRING_AGG(Character, ', ') from FilmCreditsJunction mg where mg.FilmID = @FilmID AND mg.PersonID = g.TMDB_ID) as Character
                ,(select TOP 1 ListOrder from FilmCreditsJunction mg where mg.FilmID = @FilmID AND mg.PersonID = g.TMDB_ID) as ListOrder
                FROM People as g 
                WHERE g.TMDB_ID in
                (Select PersonID from FilmCreditsJunction mg where mg.FilmID = @FilmID AND mg.Character IS NOT NULL)
                ORDER BY ListOrder ASC";

                using (var con = GetConnection)
                {
                    var result = await con.QueryAsync<People>(query, param);
                    return result.ToList();
                }
            }
            catch (Exception ex)
            {
                LogsRepository.CreateLog(ex);
                return null;
            }
        }
        public async Task<List<People>> GetCrew(int FilmID)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@FilmID", FilmID);

                string query = $@"
                SELECT * 
                ,(Select STRING_AGG(Department, ', ') from FilmCreditsJunction mg where mg.FilmID = @FilmID AND mg.PersonID = g.TMDB_ID) as Department
                ,(Select STRING_AGG(Job, ', ') from FilmCreditsJunction mg where mg.FilmID = @FilmID AND mg.PersonID = g.TMDB_ID) as Job
                FROM People as g 
                WHERE g.TMDB_ID in
                (Select PersonID from FilmCreditsJunction mg where mg.FilmID = @FilmID AND Character IS NULL)
                ORDER BY TMDB_ID ASC";

                using (var con = GetConnection)
                {
                    var result = await con.QueryAsync<People>(query, param);
                    return result.ToList();
                }
            }
            catch (Exception ex)
            {
                LogsRepository.CreateLog(ex);
                return null;
            }
        }

        public async Task<List<People>> Import(List<People> entity)
        {
            var result = new List<People>();
            try
            {
                using (var con = GetConnection)
                {
                    foreach (var item in entity)
                    {

                        DynamicParameters param = new DynamicParameters();
                        param.Add("@TMDB_ID", item.TMDB_ID);
                        param.Add("@Name", item.Name);
                        param.Add("@Original_Name", item.Original_Name);
                        param.Add("@Photo_URL", item.Photo_URL);
                        param.Add("@Profession", item.Profession);
                        param.Add("@Gender", item.Gender);
                        param.Add("@IsAdult", item.IsAdult);

                        string query = $@"
                        DECLARE  @result table(ID Int, TMDB_ID Int, Name nvarchar(MAX), Original_Name nvarchar(MAX), Photo_URL nvarchar(MAX), Profession nvarchar(250), Gender Int, IsAdult bit)
                        IF EXISTS(SELECT * from People where TMDB_ID = @TMDB_ID)        
                        BEGIN            
                        UPDATE People
                                    SET TMDB_ID = @TMDB_ID, Name = @Name, Original_Name = @Original_Name, Photo_URL = @Photo_URL, Profession = @Profession, Gender = @Gender, IsAdult = @IsAdult
							        OUTPUT INSERTED.* INTO @result
                                    WHERE TMDB_ID = @TMDB_ID;
                        END                    
                        ELSE            
                        BEGIN  
                        INSERT INTO People (TMDB_ID, Name, Original_Name, Photo_URL, Profession, Gender,  IsAdult)
                                     OUTPUT INSERTED.* INTO @result
                                     VALUES (@TMDB_ID, @Name, @Original_Name, @Photo_URL, @Profession, @Gender, @IsAdult)
                        END
                        SELECT *
				        FROM @result";

                        var res = await con.QueryFirstOrDefaultAsync<People>(query, param);
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

        public async Task<ProcessResult> Update(People entity)
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
