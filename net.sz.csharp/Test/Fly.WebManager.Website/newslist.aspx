<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="newslist.aspx.cs" Inherits="Fly.WebManager.Website.newslist" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headstyle" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headscript" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="content" runat="server">
    <div class="container">
        <div id="button" class="mt10">
            <input type="button" name="button" class="btn btn82 btn_add" value="新增">
            <input type="button" name="button" class="btn btn82 btn_del" value="删除">
            <input type="button" name="button" class="btn btn82 btn_config" value="配置">
            <input type="button" name="button" class="btn btn82 btn_count" value="统计">
            <input type="button" name="button" class="btn btn82 btn_checked" value="全选">
            <input type="button" name="button" class="btn btn82 btn_nochecked" value="取消">
            <input type="button" name="button" class="btn btn82 btn_export" value="导出">
            <input type="button" name="button" class="btn btn82 btn_recycle" value="回收站">
            <input type="button" name="button" class="btn btn82 btn_search" value="查询">
        </div>
        <div id="search_bar" class="mt10">
            <div class="box">
                <div class="box_border">
                    <div class="box_top">
                        <b class="pl15">搜索</b></div>
                    <div class="box_center pt10 pb10">
                        <table class="form_table" border="0" cellpadding="0" cellspacing="0">
                            <tr>
                                <td>
                                    姓名
                                </td>
                                <td>
                                    <input type="text" name="name" class="input-text lh25" size="10">
                                </td>
                                <td>
                                    性别
                                </td>
                                <td>
                                    <span class="fl">
                                        <div class="select_border">
                                            <div class="select_containers ">
                                                <select name="" class="select">
                                                    <option>男</option>
                                                    <option>女</option>
                                                </select>
                                            </div>
                                        </div>
                                    </span>
                                </td>
                                <td>
                                    姓名
                                </td>
                                <td>
                                    <input type="text" name="name" class="input-text lh25" size="10">
                                </td>
                                <td>
                                    性别
                                </td>
                                <td>
                                    <span class="fl">
                                        <div class="select_border">
                                            <div class="select_containers ">
                                                <select name="" class="select">
                                                    <option>男</option>
                                                    <option>女</option>
                                                </select>
                                            </div>
                                        </div>
                                    </span>
                                </td>
                                <td>
                                    姓名
                                </td>
                                <td>
                                    <input type="text" name="name" class="input-text lh25" size="10">
                                </td>
                                <td>
                                    性别
                                </td>
                                <td>
                                    <span class="fl">
                                        <div class="select_border">
                                            <div class="select_containers ">
                                                <select name="" class="select">
                                                    <option>男</option>
                                                    <option>女</option>
                                                </select>
                                            </div>
                                        </div>
                                    </span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    姓名
                                </td>
                                <td>
                                    <input type="text" name="name" class="input-text lh25" size="10">
                                </td>
                                <td>
                                    性别
                                </td>
                                <td>
                                    <span class="fl">
                                        <div class="select_border">
                                            <div class="select_containers ">
                                                <select name="" class="select">
                                                    <option>男</option>
                                                    <option>女</option>
                                                </select>
                                            </div>
                                        </div>
                                    </span>
                                </td>
                                <td>
                                    姓名
                                </td>
                                <td>
                                    <input type="text" name="name" class="input-text lh25" size="10">
                                </td>
                                <td>
                                    性别
                                </td>
                                <td>
                                    <span class="fl">
                                        <div class="select_border">
                                            <div class="select_containers ">
                                                <select name="" class="select">
                                                    <option>男</option>
                                                    <option>女</option>
                                                </select>
                                            </div>
                                        </div>
                                    </span>
                                </td>
                                <td>
                                    姓名
                                </td>
                                <td>
                                    <input type="text" name="name" class="input-text lh25" size="10">
                                </td>
                                <td>
                                    性别
                                </td>
                                <td>
                                    <span class="fl">
                                        <div class="select_border">
                                            <div class="select_containers ">
                                                <select name="" class="select">
                                                    <option>男</option>
                                                    <option>女</option>
                                                </select>
                                            </div>
                                        </div>
                                    </span>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="box_bottom pb5 pt5 pr10" style="border-top: 1px solid #dadada;">
                        <div class="search_bar_btn" style="text-align: right;">
                            <input type="submit" value="确定" class="ext_btn ext_btn_submit">
                            <input type="button" value="返回" onclick="location.href='javascript:history.go(-1)'"
                                class="ext_btn">
                            <input type="button" class="ext_btn ext_btn_success" value="成功">
                            <input type="button" class="ext_btn ext_btn_error" value="错误">
                            <a href="" class="ext_btn"><span class="add"></span>添加</a>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="table" class="mt10">
            <div class="box span10 oh">
                <table width="100%" border="0" cellpadding="0" cellspacing="0" class="list_table">
                    <tr>
                        <th width="30">
                            #
                        </th>
                        <th width="100">
                            标题
                        </th>
                        <th width="100">
                            标题
                        </th>
                        <th>
                            标题
                        </th>
                    </tr>
                    <tr class="tr">
                        <td class="td_center">
                            <input type="checkbox">
                        </td>
                        <td>
                            aad
                        </td>
                        <td>
                            aad
                        </td>
                        <td>
                            aad
                        </td>
                    </tr>
                </table>
                <div class="page mt10">
                    <div class="pagination">
                        <ul>
                            <li class="first-child"><a href="#">首页</a></li>
                            <li class="disabled"><span>上一页</span></li>
                            <li class="active"><span>1</span></li>
                            <li><a href="#">2</a></li>
                            <li><a href="#">2</a></li>
                            <li><a href="#">2</a></li>
                            <li><a href="#">2</a></li>
                            <li><a href="#">下一页</a></li>
                            <li class="last-child"><a href="#">末页</a></li>
                        </ul>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
