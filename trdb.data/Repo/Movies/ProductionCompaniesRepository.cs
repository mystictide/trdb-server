using Dapper;
using Dapper.Contrib.Extensions;
using trdb.data.Interface.Movies;
using trdb.data.Repo.Helpers;
using trdb.entity.Helpers;
using trdb.entity.Movies;

namespace trdb.data.Repo.Movies
{
    public class ProductionCompaniesRepository : Connection.dbConnection, IProductionCompanies
    {
        public async Task<ProductionCompanies> Add(ProductionCompanies entity)
        {
            ProcessResult result = new ProcessResult();
            try
            {
                using (var con = GetConnection)
                {
                    result.ReturnID = await con.InsertAsync(entity);
                    result.Message = "Company saved successfully";
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
                    await con.DeleteAsync(new ProductionCompanies() { ID = ID });
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

        public async Task<FilteredList<ProductionCompanies>> FilteredList(FilteredList<ProductionCompanies> request)
        {
            try
            {
                FilteredList<ProductionCompanies> result = new FilteredList<ProductionCompanies>();
                DynamicParameters param = new DynamicParameters();
                param.Add("@Keyword", request.filter.Keyword);
                param.Add("@PageSize", request.filter.pageSize);

                string WhereClause = @" WHERE (t.Name like '%' + @Keyword + '%')";

                string query_count = $@"  Select Count(t.ID) from ProductionCompanies t {WhereClause}";

                string query = $@"
                SELECT *
                FROM ProductionCompanies t
                {WhereClause} 
                ORDER BY t.ID ASC 
                OFFSET @StartIndex ROWS
                FETCH NEXT @PageSize ROWS ONLY";

                using (var con = GetConnection)
                {
                    result.totalItems = await con.QueryFirstOrDefaultAsync<int>(query_count, param);
                    request.filter.pager = new Page(result.totalItems, request.filter.pageSize, request.filter.page);
                    param.Add("@StartIndex", request.filter.pager.StartIndex);
                    result.data = await con.QueryAsync<ProductionCompanies>(query, param);
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

        public async Task<ProductionCompanies> Get(int ID)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@ID", ID);

                string query = $@"
                SELECT *
                FROM ProductionCompanies 
                WHERE ID = @ID";

                using (var con = GetConnection)
                {
                    var res = await con.QueryAsync<ProductionCompanies>(query, param);
                    return res.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                LogsRepository.CreateLog(ex);
                return null;
            }
        }

        public async Task<IEnumerable<ProductionCompanies>> GetAll()
        {
            try
            {
                using (var con = GetConnection)
                {
                    var res = await con.GetAllAsync<ProductionCompanies>();
                    return res;
                }
            }
            catch (Exception ex)
            {
                LogsRepository.CreateLog(ex);
                return null;
            }
        }

        public async Task<List<ProductionCompanies>> GetMovieCompanies(int MovieID)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@MovieID", MovieID);

                string query = $@"
                SELECT * FROM ProductionCompanies as pc 
                WHERE pc.TMDB_ID in
                (Select ProductionCompanyID from MovieProductionCompanyJunction mpc where mpc.MovieID = @MovieID)
                ORDER BY TMDB_ID ASC";

                using (var con = GetConnection)
                {
                    var result = await con.QueryAsync<ProductionCompanies>(query, param);
                    return result.ToList();
                }
            }
            catch (Exception ex)
            {
                LogsRepository.CreateLog(ex);
                return null;
            }
        }

        public async Task<List<ProductionCompanies>> Import(List<ProductionCompanies> entity)
        {
            var result = new List<ProductionCompanies>();
            try
            {
                using (var con = GetConnection)
                {
                    foreach (var item in entity)
                    {
                        DynamicParameters param = new DynamicParameters();
                        param.Add("@TMDB_ID", item.TMDB_ID);
                        param.Add("@Name", item.Name);
                        param.Add("@Logo_URL", item.Logo_URL);
                        param.Add("@Origin", item.Origin);

                        string query = $@"
                       DECLARE  @result table(ID Int, TMDB_ID Int, Name nvarchar(MAX), Logo_URL nvarchar(MAX), Origin nvarchar(100))
                        IF EXISTS(SELECT * from ProductionCompanies where TMDB_ID = @TMDB_ID)        
                        BEGIN            
                        UPDATE ProductionCompanies
                                    SET TMDB_ID = @TMDB_ID, Name = @Name, Logo_URL = @Logo_URL, Origin = @Origin
							        OUTPUT INSERTED.* INTO @result
                                    WHERE TMDB_ID = @TMDB_ID;
                        END                    
                        ELSE            
                        BEGIN  
                        INSERT INTO ProductionCompanies (TMDB_ID, Name, Logo_URL, Origin)
                                     OUTPUT INSERTED.* INTO @result
                                     VALUES (@TMDB_ID, @Name, @Logo_URL, @Origin)
                        END
                        SELECT *
				        FROM @result";

                        var res = await con.QueryFirstOrDefaultAsync<ProductionCompanies>(query, param);
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

        public async Task<ProcessResult> Update(ProductionCompanies entity)
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
                        result.Message = "Company updated successfully.";
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
