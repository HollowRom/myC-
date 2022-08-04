using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Kingdee.BOS;
using Kingdee.BOS.Util;
using Kingdee.BOS.Core;
using Kingdee.BOS.Core.DynamicForm.PlugIn;
using Kingdee.BOS.Core.DynamicForm.PlugIn.Args;
using Kingdee.BOS.Core.DynamicForm.PlugIn.ControlModel;
using Kingdee.BOS.Core.List;
using Kingdee.BOS.Core.List.PlugIn;
using Kingdee.BOS.Core.List.PlugIn.Args;
using Kingdee.BOS.Core.Metadata;
using Kingdee.BOS.Orm.DataEntity;
namespace JDSample.FormPlugIn.BaseData {
    /// 多层次基础资料列表插件    
    /// 演示用基础资料：   
    /// FormId : 08441f27-444c-43ee-bed9-8498a1ff066c   
    /// 基类：1.1 不受组织控制基础资料模板(BOS_NoOrgControlBDModel)   
    /// 名称：资料类别
    /// 包含的字段：
    /// 1. 上级资料 F_JD_ParentId
    /// 2. 编码 FNumber
    /// 3. 名称 FName
    /// 4. 完整父节点内码 F_JD_FullParentId （由各级父节点内码组成，如(.1001.1002.1003，以便根据当前节点，快速找出所有下级节点） 
    [Description("多层次基础资料列表插件")]
    public class MulLevelBaseDataList : AbstractListPlugIn
    {
        private TreeNode _groupRootNode = null;    
        /// 界面请求构建树节点事件；拦截此事件，自行构建树节点     
        public override void TreeLoadData(TreeLoadDataArgs e)
        {
            TreeView tv = (TreeView)this.View.GetControl("FGroupTreeView");
            var parentNode = BuildTreeNode(this.Context, this.View.BillBusinessInfo);
            tv.SetRootNode(parentNode);
            tv.SetExpanded(true);            // 略过系统预置取分组树的功能        
            e.Cancel  = true;
        }      
        /// 用户点击树节点事件：  
        /// 拦截此事件，根据用户所选节点，设置过滤条件，筛选列表数据     
        public override void TreeNodeClick(TreeNodeArgs e)
        {
            TreeViewFilterParameter filterParameter = ((ITreeListModel)this.Model).TreeViewFilterParameter as TreeViewFilterParameter;
            filterParameter.FilterString = string.Empty;
            // 分组过滤条件           
            filterParameter.IgnoreSelectedGroupIds = false;
            // 是否忽略默认的分组过滤处理        
            if (e.NodeId == "0")
            {
                // 点击根节点，无需设置分组过滤条件                
                return;
            }
            // 拼接分组过滤条件：资料类别 = 本节点，以及本节点的全部下级节点           
            string filter = string.Format(" (FID = {0} OR F_JD_FullParentId LIKE '%.{0}.%') ", e.NodeId);
            // 采用二开设置的分组过滤条件，忽略系统默认的分组过滤        
            filterParameter.FilterString = filter;
            filterParameter.IgnoreSelectedGroupIds = true;
        }
        public static TreeNode BuildTreeNode(Context ctx, BusinessInfo groupInfo)
        {
            // 构建根目录       
            TreeNode rootNode = new TreeNode();
            rootNode.id = "0";
            rootNode.parentid = "0";
            rootNode.text = "全部";
            // 读取全部资料分类数据    
            List<SelectorItemInfo> selectorList = new List<SelectorItemInfo>();
            selectorList.Add(new SelectorItemInfo("FID"));
            selectorList.Add(new SelectorItemInfo("F_JD_ParentId"));
            selectorList.Add(new SelectorItemInfo("FNumber"));
            selectorList.Add(new SelectorItemInfo("FName"));
            var infoGroups = Kingdee.BOS.ServiceHelper.BusinessDataServiceHelper.Load(      
                ctx,                     
                groupInfo,              
                selectorList,         
                OQLFilter.CreateHeadEntityFilter(""));
            if (infoGroups == null || infoGroups.Length == 0) return rootNode;
            // 把各资料分类，转换为树节点，并放在字典中       
            Dictionary<string, TreeNode> dctNodes = new Dictionary<string, TreeNode>();
            foreach (var infoGroup in infoGroups)
            {
                long id = Convert.ToInt64(infoGroup["Id"]);
                long parentId = Convert.ToInt64(infoGroup["F_JD_ParentId_Id"]);
                string number = Convert.ToString(infoGroup["Number"]);
                string name = Convert.ToString(infoGroup["Name"]);
                TreeNode node = new TreeNode()
                {
                    id = id.ToString(),
                    text = string.Format("{0}({1})", name, number),
                    parentid = parentId.ToString(),
                };
                dctNodes.Add(id.ToString(), node);
            }
            // 把各节点，放在其父节点下面         
            foreach (var item in dctNodes)
            {
                TreeNode node = item.Value;
                if (node.parentid == "0")
                {
                    rootNode.children.Add(node);
                }
                else if (dctNodes.Keys.Contains(node.parentid))
                {
                    dctNodes[node.parentid].children.Add(node);
                }
            }
            return rootNode;
        }
    }
}
