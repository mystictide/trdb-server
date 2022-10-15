using Dapper.Contrib.Extensions;
using Dapper;
using trdb.data.Interface.Movies;
using trdb.data.Repo.Helpers;
using trdb.entity.Helpers;
using trdb.entity.Movies;

namespace trdb.data.Repo.Movies
{
    public class ProductionCountriesRepository : Connection.dbConnection, IProductionCountries
    {
        public async Task<ProductionCountries> Add(ProductionCountries entity)
        {
            ProcessResult result = new ProcessResult();
            try
            {
                using (var con = GetConnection)
                {
                    result.ReturnID = await con.InsertAsync(entity);
                    result.Message = "Country saved successfully";
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
                    await con.DeleteAsync(new ProductionCountries() { ID = ID });
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

        public async Task<FilteredList<ProductionCountries>> FilteredList(FilteredList<ProductionCountries> request)
        {
            try
            {
                FilteredList<ProductionCountries> result = new FilteredList<ProductionCountries>();
                DynamicParameters param = new DynamicParameters();
                param.Add("@Keyword", request.filter.Keyword);
                param.Add("@PageSize", request.filter.pageSize);

                string WhereClause = @" WHERE (t.Name like '%' + @Keyword + '%')";

                string query_count = $@"  Select Count(t.ID) from ProductionCountries t {WhereClause}";

                string query = $@"
                SELECT *
                FROM ProductionCountries t
                {WhereClause} 
                ORDER BY t.ID ASC 
                OFFSET @StartIndex ROWS
                FETCH NEXT @PageSize ROWS ONLY";

                using (var con = GetConnection)
                {
                    result.totalItems = await con.QueryFirstOrDefaultAsync<int>(query_count, param);
                    request.filter.pager = new Page(result.totalItems, request.filter.pageSize, request.filter.page);
                    param.Add("@StartIndex", request.filter.pager.StartIndex);
                    result.data = await con.QueryAsync<ProductionCountries>(query, param);
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

        public async Task<ProductionCountries> Get(int ID)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@ID", ID);

                string query = $@"
                SELECT *
                FROM ProductionCountries 
                WHERE ID = @ID";

                using (var con = GetConnection)
                {
                    var res = await con.QueryAsync<ProductionCountries>(query, param);
                    return res.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                LogsRepository.CreateLog(ex);
                return null;
            }
        }

        public async Task<IEnumerable<ProductionCountries>> GetAll()
        {
            try
            {
                using (var con = GetConnection)
                {
                    var res = await con.GetAllAsync<ProductionCountries>();
                    return res;
                }
            }
            catch (Exception ex)
            {
                LogsRepository.CreateLog(ex);
                return null;
            }
        }

        public async Task<List<ProductionCountries>> Import(List<ProductionCountries> entity)
        {
            var result = new List<ProductionCountries>();
            foreach (var item in entity)
            {
                try
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@Name", item.Name);
                    param.Add("@iso", item.iso_3166_1);

                    string query = $@"
                    DECLARE  @result table(ID Int, Name nvarchar(MAX), iso_3166_1 nvarchar(5))
                    IF EXISTS(SELECT * from ProductionCountries where Name = @Name)   
                    BEGIN            
                    UPDATE ProductionCountries
                                SET Name = @Name, iso_3166_1 = @iso
							    OUTPUT INSERTED.* INTO @result
                                WHERE Name = @Name;
                    END                    
                    ELSE            
                    BEGIN  
                    INSERT INTO ProductionCountries (Name, iso_3166_1)
                                 OUTPUT INSERTED.* INTO @result
                                 VALUES (@Name, @iso)
                    END
                    SELECT *
				    FROM @result";

                    using (var con = GetConnection)
                    {
                        var res = await con.QueryFirstOrDefaultAsync<ProductionCountries>(query, param);
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

        public async Task<ProcessResult> Update(ProductionCountries entity)
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
