var clients = [];
$(function () {
    clients = $.clientsInit();
})
$.clientsInit = function () {
    var dataJson = {
        dataItems: [],
        organize: [],
        company: [],
        role: [],
        duty: [],
        user: [],
        authorizeButton: [],
        authorizeFields: [],
        template: [],
        hospital: [],
        moduleFields: [],
        channel: [],
        lable: [],
        expert: [],
        templateType:[]
    };
    var init = function () {
        $.ajax({
            url: "../ClientsData/GetClientsDataJson?v="+new Date().Format("yyyy-MM-dd hh:mm:ss"),
            type: "get",
            dataType: "json",
            async: false,
            success: function (data) {
                dataJson.dataItems = data.dataItems;
                dataJson.organize = data.organize;
                dataJson.role = data.role;
                dataJson.duty = data.duty;
                dataJson.user = data.user;
                dataJson.authorizeButton = data.authorizeButton;
                dataJson.authorizeFields = data.authorizeFields;
                dataJson.moduleFields = data.moduleFields;
                dataJson.company = data.company;
                dataJson.template = data.template;
                dataJson.hospital = data.hospital;
                dataJson.channel = data.channel;
                dataJson.lable = data.lable;
                dataJson.expert = data.expert;
                dataJson.templateType = data.templateType;
            }
        });
    }
    init();
    return dataJson;
}