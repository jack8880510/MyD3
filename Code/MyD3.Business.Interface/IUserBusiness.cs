using MyD3.Business.Interface.DataModel.User;
using MyD3.Entity;
using MyD3.Entity.DBEntity;

namespace MyD3.Business.Interface
{
    /// <summary>
    /// 用户业务模型接口
    /// </summary>
    public interface IUserBusiness
    {
        /// <summary>
        /// 根据系统编号获取用户信息对象
        /// </summary>
        /// <param name="id">系统编号</param>
        /// <returns>用户信息对象</returns>
        ResultValue<User> Get(string id);

        /// <summary>
        /// 保存用户信息，如果用户存在则是编辑，否则是新增
        /// </summary>
        /// <param name="user">需要保存的用户信息</param>
        /// <param name="creator">创建人</param>
        /// <returns>用户信息保存结果</returns>
        ResultValue<User> Save(User user, User creator);

        /// <summary>
        /// 验证指定的登录名是否可用于该指定编号的注册
        /// </summary>
        /// <param name="loginName">需要验证的登录名</param>
        /// <param name="id">用户编号</param>
        /// <returns>是否可用</returns>
        ResultValue<bool> LoginNameValidate(string loginName, string id);

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="loginArgs">登录参数</param>
        /// <returns>登录结果，Code不为0时则为失败，为0则Data信息中包含登录用户的数据及其授权信息</returns>
        ResultValue<LoginInfo> Login(LoginArgs loginArgs);

        /// <summary>
        /// 根据搜索条件，搜索用户信息
        /// </summary>
        /// <param name="args">搜索条件</param>
        /// <returns>搜索结果</returns>
        ResultValue<SearchResult<User>> Search(SearchArgs args);

        /// <summary>
        /// 删除指定的用户信息
        /// </summary>
        /// <param name="user">需要删除的用户对象</param>
        /// <param name="modify_user">操作用户</param>
        /// <returns>是否删除成功</returns>
        ResultValue<bool> Delete(User user, User modify_user);

        /// <summary>
        /// 获取指定分组下的成员列表
        /// </summary>
        /// <param name="groupID">分组编号</param>
        /// <returns>成员信息集合</returns>
        ResultValue<User[]> GetGroupMembers(string groupID);
    }
}
