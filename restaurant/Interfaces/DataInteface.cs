interface DataInteface
{
    Task<object> GetAll();
    Task<object> Get(object name_id);
    Task<object> Post(object data);
    Task<object> Update(object name_id, object data);
    Task<object> Delete(object name_id);
}