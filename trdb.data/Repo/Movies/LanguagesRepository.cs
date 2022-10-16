using Dapper.Contrib.Extensions;
using Dapper;
using trdb.data.Interface.Movies;
using trdb.data.Repo.Helpers;
using trdb.entity.Helpers;
using trdb.entity.Movies;

namespace trdb.data.Repo.Movies
{
    public class LanguagesRepository : Connection.dbConnection, ILanguages
    {
        public async Task<Languages> Add(Languages entity)
        {
            ProcessResult result = new ProcessResult();
            try
            {
                using (var con = GetConnection)
                {
                    result.ReturnID = await con.InsertAsync(entity);
                    result.Message = "Language saved successfully";
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
                    await con.DeleteAsync(new Languages() { ID = ID });
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

        public async Task<FilteredList<Languages>> FilteredList(FilteredList<Languages> request)
        {
            try
            {
                FilteredList<Languages> result = new FilteredList<Languages>();
                DynamicParameters param = new DynamicParameters();
                param.Add("@Keyword", request.filter.Keyword);
                param.Add("@PageSize", request.filter.pageSize);

                string WhereClause = @" WHERE (t.Name like '%' + @Keyword + '%')";

                string query_count = $@"  Select Count(t.ID) from Languages t {WhereClause}";

                string query = $@"
                SELECT *
                FROM Languages t
                {WhereClause} 
                ORDER BY t.ID ASC 
                OFFSET @StartIndex ROWS
                FETCH NEXT @PageSize ROWS ONLY";

                using (var con = GetConnection)
                {
                    result.totalItems = await con.QueryFirstOrDefaultAsync<int>(query_count, param);
                    request.filter.pager = new Page(result.totalItems, request.filter.pageSize, request.filter.page);
                    param.Add("@StartIndex", request.filter.pager.StartIndex);
                    result.data = await con.QueryAsync<Languages>(query, param);
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

        public async Task<Languages> Get(int ID)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@ID", ID);

                string query = $@"
                SELECT *
                FROM Languages 
                WHERE ID = @ID";

                using (var con = GetConnection)
                {
                    var res = await con.QueryAsync<Languages>(query, param);
                    return res.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                LogsRepository.CreateLog(ex);
                return null;
            }
        }

        public async Task<IEnumerable<Languages>> GetAll()
        {
            try
            {
                using (var con = GetConnection)
                {
                    var res = await con.GetAllAsync<Languages>();
                    return res;
                }
            }
            catch (Exception ex)
            {
                LogsRepository.CreateLog(ex);
                return null;
            }
        }

        public async Task<List<Languages>> GetMovieLanguages(int MovieID)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@MovieID", MovieID);

                string query = $@"
                SELECT * FROM Languages as l
                WHERE l.ID in 
                (Select LanguageID from MovieLanguageJunction ml where ml.MovieID = @MovieID)
                ORDER BY ID ASC";

                using (var con = GetConnection)
                {
                    var result = await con.QueryAsync<Languages>(query, param);
                    return result.ToList();
                }
            }
            catch (Exception ex)
            {
                LogsRepository.CreateLog(ex);
                return null;
            }
        }

        public async Task<List<Languages>> Import(List<Languages> entity)
        {
            var result = new List<Languages>();
            foreach (var item in entity)
            {
                try
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@Name", item.Name);

                    string query = $@"
                    DECLARE  @result table(ID Int, Name nvarchar(100))
                    IF EXISTS(SELECT * from Languages where Name = @Name)        
                    BEGIN            
                    UPDATE Languages
                                SET Name = @Name
							    OUTPUT INSERTED.* INTO @result
                                WHERE Name = @Name;
                    END                    
                    ELSE            
                    BEGIN  
                    INSERT INTO Languages (Name)
                                 OUTPUT INSERTED.* INTO @result
                                 VALUES (@Name)
                    END
                    SELECT *
				    FROM @result";

                    using (var con = GetConnection)
                    {
                        var res = await con.QueryFirstOrDefaultAsync<Languages>(query, param);
                        result.Add(res);
                    }
                }
                catch (Exception ex)
                {
                    LogsRepository.CreateLog(ex);
                }
            }
            return result;
        }

        public async Task<ProcessResult> Update(Languages entity)
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
                        result.Message = "Language updated successfully.";
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
