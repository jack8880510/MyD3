using MyD3.Business.Interface;
using MyD3.Common.Log;
using MyD3.Data.Interface;
using MyD3.Entity;
using MyD3.Entity.DBEntity;
using System.Collections.Generic;
using MyD3.Business.Interface.DataModel.User;
using System;
using Newtonsoft.Json;
using System.Collections;
using System.Linq;
using MyD3.Common.Data;
using System.Transactions;

namespace MyD3.Business
{
    /// <summary>
    /// 分组业务模型
    /// </summary>
    public class GroupBusiness : IGroupBusiness
    {
        private readonly IRepository<Group> _groupRps;
        private readonly IRepository<UserGroup> _userGroupRps;
        private readonly ILogger _logger;

        /// <summary>
        /// 初始化分组业务模型
        /// </summary>
        /// <param name="groupRps">分组数据仓库</param>
        /// <param name="logger">日志管理</param>
        public GroupBusiness(IRepository<Group> groupRps,
            IRepository<UserGroup> userGroupRps, ILogger logger)
        {
            this._groupRps = groupRps;
            this._logger = logger;
            this._userGroupRps = userGroupRps;
        }

        /// <summary>
        /// 获取分组信息
        /// </summary>
        /// <param name="id">分组编号</param>
        /// <returns>分组信息</returns>
        public ResultValue<Group> Get(string id)
        {
            this._logger.I("开始获取分组信息：" + id);

            if (string.IsNullOrEmpty(id))
            {
                this._logger.I("由于参数id为空，获取分组信息失败了");
                return new ResultValue<Group>(-1, "参数ID不能为空");
            }

            var data = this._groupRps.Table.Where(x => x.ID == id && x.Status != -1).FirstOrDefault();

            return new ResultValue<Group>(data);
        }

        /// <summary>
        /// 验证指定的分组名称是否可用于该指定编号的新增
        /// </summary>
        /// <param name="name">需要验证的分组名</param>
        /// <param name="id">分组编号</param>
        /// <returns>是否可用</returns>
        public ResultValue<bool> GroupNameValidate(string name, string id)
        {
            this._logger.I("开始判断分组名是否已经存在了");
            var existsGroup = this._groupRps.Table.Where(x => x.Name == name && x.Status != -1).FirstOrDefault();
            if (existsGroup != null && existsGroup.ID != id)
            {
                return new ResultValue<bool>(false);
            }
            else
            {
                return new ResultValue<bool>(true);
            }
        }

        /// <summary>
        /// 获取用户关联的分组
        /// </summary>
        /// <param name="userID">用户编号</param>
        /// <returns>分组信息集合</returns>
        public ResultValue<Group[]> GetUserGroup(string userID)
        {
            this._logger.I("开始获取用户分组信息，用户编号为：" + userID);

            if (string.IsNullOrEmpty(userID))
            {
                this._logger.I("由于参数userID为空，获取用户分组信息失败了");
                return new ResultValue<Group[]>(-1, "参数userID不能为空");
            }

            var resultData = this._groupRps.Table
                .Join(this._userGroupRps.Table, g => g.ID, ug => ug.GroupID, (g, ug) => new { Group = g, UserID = ug.UserID })
                .Where(x => x.UserID == userID && x.Group.Status != -1)
                .Select(x => x.Group).ToArray();

            return new ResultValue<Group[]>(resultData);
        }

        /// <summary>
        /// 保存分组信息，如果分组存在则是编辑，否则是新增
        /// </summary>
        /// <param name="group">需要保存的分组信息</param>
        /// <param name="creator">创建人</param>
        /// <returns>分组信息保存结果</returns>
        public ResultValue<Group> Save(Group group, User creator)
        {
            this._logger.I("开始分组保存");
            this._logger.D(JsonConvert.SerializeObject(group));

            if (group == null)
            {
                this._logger.W("由于分组对象不存在，保存失败了");
                return new ResultValue<Group>(-1, "需要保存的分组对象为空，保存失败");
            }

            if (string.IsNullOrEmpty(group.Name) || group.Name.Length < 2 || group.Name.Length > 25)
            {
                this._logger.W("分组名称不合法，保存失败：" + group.Name);
                return new ResultValue<Group>(-1, "分组名称不合法,长度必须在2-25位之间，保存失败");
            }

            this._logger.I("开始判断分组名是否已经存在了");
            var existsGroupResult = this.GroupNameValidate(group.Name, group.ID);
            if (existsGroupResult.RCode != 0 || !existsGroupResult.Data)
            {
                this._logger.W("分组名唯一性验证失败：" + existsGroupResult.ToString());
                return new ResultValue<Group>(-1, "分组名唯一性验证失败，保存失败");
            }

            this._logger.I("验证成功开始最后的数据封装");
            if (string.IsNullOrEmpty(group.ID))
            {
                group.CreateDate = DateTime.Now;
                group.CreatorID = creator.ID;
                group.ID = Guid.NewGuid().ToString();
                this._groupRps.Insert(group);
            }
            else
            {
                //如果是编辑，则需要从数据库从新读取分组数据
                var getGroupResult = this.Get(group.ID);
                if (getGroupResult.RCode != 0)
                {
                    this._logger.W("无法从数据获取分组原始数据，更新失败：" + getGroupResult.ToString());
                    return new ResultValue<Group>(-1, "无法从数据获取分组原始数据，保存失败");
                }

                getGroupResult.Data.Name = group.Name;
                getGroupResult.Data.ModifyDate = new Nullable<DateTime>(DateTime.Now);
                getGroupResult.Data.ModifyUser = creator.ID;
                this._groupRps.Update(getGroupResult.Data);
                group = getGroupResult.Data;
            }
            return new ResultValue<Group>(group);
        }

        /// <summary>
        /// 根据搜索条件，搜索分组信息
        /// </summary>
        /// <param name="args">搜索条件</param>
        /// <returns>搜索结果</returns>
        public ResultValue<SearchResult<Group>> Search(SearchArgs args)
        {
            this._logger.I("开始业务层搜索处理");
            this._logger.D("开始创建queryable对象");
            var query = this._groupRps.Table.Where(x => x.Status != -1);

            if (!string.IsNullOrEmpty(args.ConditionString))
            {
                this._logger.D("开始创建name条件");
                query = query.Where(x => x.Name.Contains(args.ConditionString));
            }

            this._logger.D("开始数据库查询");
            var data = query.OrderBy(args.SortName, !args.SortDESC)
                .Skip(args.PageSize * args.PageIndex)
                .Take(args.PageSize)
                .ToArray();

            this._logger.D("开始获取结果的总数量");
            var count = query.Count();

            this._logger.D("开始封装成为查询结果对象");
            var result = new SearchResult<Group>(args, data, count);

            this._logger.D("返回查询结果");
            return new ResultValue<SearchResult<Group>>(result);
        }

        /// <summary>
        /// 删除指定的分组信息
        /// </summary>
        /// <param name="group">需要删除的分组对象</param>
        /// <param name="modify_user">创建人</param>
        /// <returns>是否删除成功</returns>
        public ResultValue<bool> Delete(Group group, User modify_user)
        {
            this._logger.I("开始删除分组");
            this._logger.D(JsonConvert.SerializeObject(group));

            if (group == null)
            {
                this._logger.W("由于分组对象不存在，删除失败");
                return new ResultValue<bool>(-1, "由于分组对象不存在，删除失败");
            }
            else if (string.IsNullOrEmpty(group.ID))
            {
                this._logger.W("该分组ID信息有误，删除失败");
                return new ResultValue<bool>(-1, "该分组ID信息有误，删除失败");
            }

            //根据编号获取分组对象
            var getGroupResult = this.Get(group.ID);

            if (getGroupResult.RCode != 0)
            {
                this._logger.W("根据分组ID获取分组失败，删除失败");
                return new ResultValue<bool>(-1, "获取分组原始信息失败，删除失败");
            }

            //开始封装对象
            group = getGroupResult.Data;
            group.ModifyDate = new Nullable<DateTime>(DateTime.Now);
            group.ModifyUser = modify_user.ID;
            group.Status = -1;   //本系统中-1代表软删除

            this._logger.I("更新");
            this._groupRps.Update(group);

            return new ResultValue<bool>(true);
        }

        /// <summary>
        /// 设置分组成员
        /// </summary>
        /// <param name="groupID">分组编号</param>
        /// <param name="memberIDList">成员ID列表</param>
        /// <param name="creator">操作员</param>
        /// <returns>是否设置成功</returns>
        public ResultValue<bool> SetGroupMember(string groupID, string[] memberIDList, User creator)
        {
            this._logger.I("开始进行分组成员设置");
            if (string.IsNullOrEmpty(groupID))
            {
                this._logger.W("分组编号为空，无法完成设置");
                return new ResultValue<bool>(-1, "分组编号不合法，无法完成设置");
            }
            else if (memberIDList == null)
            {
                this._logger.W("分组成员为NULL，默认给空数组，做成员清空处理");
            }

            //获取分组信息
            var getGroupResult = this.Get(groupID);
            if (getGroupResult.RCode != 0 || getGroupResult.Data == null)
            {
                this._logger.W("获取分组原始数据失败，分组获取结果：" + getGroupResult);
                return new ResultValue<bool>(-1, "获取分组原始数据失败，无法完成成员设置");
            }

            var newUserGroups = new List<UserGroup>();
            if (memberIDList != null)
            {
                //转换成实体对象
                newUserGroups = memberIDList.Select(x => new UserGroup()
                {
                    CreateDate = DateTime.Now,
                    ID = Guid.NewGuid().ToString(),
                    CreatorID = creator.ID,
                    GroupID = groupID,
                    UserID = x
                }).ToList();
            }

            var existsUserGroups = this._userGroupRps.Table.Where(x => x.GroupID == groupID).ToList();

            using (TransactionScope tran = new TransactionScope())
            {
                //删除成员
                this._userGroupRps.DeleteRanges(existsUserGroups);

                if (newUserGroups.Count > 0)
                {
                    //加入新成员
                    this._userGroupRps.AddRanges(newUserGroups);
                }

                tran.Complete();
            }

            return new ResultValue<bool>(true);
        }
    }
}
