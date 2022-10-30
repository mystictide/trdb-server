using Dapper.Contrib.Extensions;
using Dapper;
using trdb.data.Repo.Helpers;
using trdb.entity.Helpers;
using trdb.entity.Movies.Junctions;
using trdb.data.Interface.Movies.Junctions;
using trdb.entity.Movies;

namespace trdb.data.Repo.Movies.Junctions
{
    public class MovieProductionCountryJunctionRepository : Connection.dbConnection, IMovieProductionCountriesJunction
    {
        public async Task<MovieProductionCountryJunction> Add(MovieProductionCountryJunction entity)
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
                    await con.DeleteAsync(new MovieProductionCountryJunction() { ID = ID });
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

        public async Task<FilteredList<MovieProductionCountryJunction>> FilteredList(FilteredList<MovieProductionCountryJunction> request)
        {
            try
            {
                FilteredList<MovieProductionCountryJunction> result = new FilteredList<MovieProductionCountryJunction>();
                DynamicParameters param = new DynamicParameters();
                param.Add("@Keyword", request.filter.Keyword);
                param.Add("@PageSize", request.filter.pageSize);

                string WhereClause = @" WHERE (t.Name like '%' + @Keyword + '%')";

                string query_count = $@"  Select Count(t.ID) from MovieProductionCountryJunction t {WhereClause}";

                string query = $@"
                SELECT *
                FROM MovieProductionCountryJunction t
                {WhereClause} 
                ORDER BY t.ID ASC 
                OFFSET @StartIndex ROWS
                FETCH NEXT @PageSize ROWS ONLY";

                using (var con = GetConnection)
                {
                    result.totalItems = await con.QueryFirstOrDefaultAsync<int>(query_count, param);
                    request.filter.pager = new Page(result.totalItems, request.filter.pageSize, request.filter.page);
                    param.Add("@StartIndex", request.filter.pager.StartIndex);
                    result.data = await con.QueryAsync<MovieProductionCountryJunction>(query, param);
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

        public async Task<MovieProductionCountryJunction> Get(int ID)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@ID", ID);

                string query = $@"
                SELECT *
                FROM MovieProductionCountryJunction 
                WHERE ID = @ID";

                using (var con = GetConnection)
                {
                    var res = await con.QueryAsync<MovieProductionCountryJunction>(query, param);
                    return res.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                LogsRepository.CreateLog(ex);
                return null;
            }
        }

        public async Task<IEnumerable<MovieProductionCountryJunction>> GetAll()
        {
            try
            {
                using (var con = GetConnection)
                {
                    var res = await con.GetAllAsync<MovieProductionCountryJunction>();
                    return res;
                }
            }
            catch (Exception ex)
            {
                LogsRepository.CreateLog(ex);
                return null;
            }
        }

        public async Task<List<ProductionCountries>> Manage(List<ProductionCountries> entity, int MovieID)
        {
            var result = new List<ProductionCountries>();
            try
            {
                using (var con = GetConnection)
                {
                    foreach (var item in entity)
                    {

                        DynamicParameters param = new DynamicParameters();
                        param.Add("@MovieID", MovieID);
                        param.Add("@ProductionCountryID", item.ID);
                        param.Add("@Name", item.Name);

                        string query = $@"
                        DECLARE  @result table(ID Int, MovieID Int, ProductionCountryID Int)
                        IF @ProductionCountryID < 1
                        BEGIN
                        SET @ProductionCountryID = (select ID from ProductionCountries where Name = @Name)
                        END
                        IF EXISTS(SELECT * from MovieProductionCountryJunction where MovieID = @MovieID AND ProductionCountryID = @ProductionCountryID)        
                        BEGIN            
                        UPDATE MovieProductionCountryJunction
                                    SET MovieID = @MovieID, ProductionCountryID = @ProductionCountryID
							        OUTPUT INSERTED.* INTO @result
                                    WHERE MovieID = @MovieID AND ProductionCountryID = @ProductionCountryID;
                        END                    
                        ELSE            
                        BEGIN  
                        INSERT INTO MovieProductionCountryJunction (MovieID, ProductionCountryID)
                                     OUTPUT INSERTED.* INTO @result
                                     VALUES (@MovieID, @ProductionCountryID)
                        END
                        SELECT *
				        FROM @result";


                        var res = await con.QueryFirstOrDefaultAsync<ProductionCountries>(query, param);
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

        public async Task<ProcessResult> Update(MovieProductionCountryJunction entity)
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
