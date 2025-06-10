interface DataInteface
{
    Task<object> GetAll();
    Task<object> Get(int id);
    Task<object> Post(object data);
    Task<object> Update(int id, object data);
    Task<object> Delete(int id);
}