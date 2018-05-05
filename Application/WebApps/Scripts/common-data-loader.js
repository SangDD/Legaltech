function cboBranchChanged(el) {
	var cboBranchRef = $("#" + $(el).attr("data-warehouseidref"));
	$(cboBranchRef).html("<option value=''>None</option>");
	var branchId = $(el).find(":selected").val();
	if(branchId != undefined && branchId != null && branchId / 0 > 0)
	$.ajax({
		type: "POST",
		url: "/commondata-loader/get-warehouse-by-branch",
		headers: { "cache-control": "no-cache" },
		data: { branchId: branchId },
		async: false,
		success: function (data) {
			if (data != null) {
				if (validateResponse(data)) {
					if (data.dataResult && data.dataResult.length > 0) {
						var htmlData = "";
						jQuery.each(data.dataResult, function (key, value) { htmlData += "<option value='" + value.WarehouseId + "'>" + value.Name + "</option>"; });
						$(cboBranchRef).html(htmlData);
					}
				}
			}
		}
	});
	$(cboBranchRef).multipleSelect("refresh");
}