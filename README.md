# ASPNet-API
Package List:
---------------------------------------------------------------------------------------------
     <PackageReference Include="Microsoft.EntityFrameworkCore" Version="3.1.8" />
     <PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="3.1.8" />
     <PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="3.1.8">
     <PackageReference Include="AutoMapper" Version="10.0.0" />
    <PackageReference Include="AutoMapper.Extensions.Microsoft.DependencyInjection" Version="8.0.1" />
    <PackageReference Include="Microsoft.AspNetCore.Mvc.Versioning" Version="4.1.1" />
    <PackageReference Include="Microsoft.AspNetCore.Mvc.Versioning.ApiExplorer" Version="4.1.1" />
  
  CSS:
  -------------------------------------------------------------------------------------------------
     <link rel="stylesheet" href="https://cdn.datatables.net/1.10.16/css/jquery.dataTables.min.css" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/jqueryui/1.12.1/jquery-ui.min.css" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/toastr.js/latest/css/toastr.min.css" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css">
    
  Javascripts File: 
  -----------------------------------------------------------------------------------------------------
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jqueryui/1.12.1/jquery-ui.min.js"></script>
    <script src="https://cdn.datatables.net/1.10.16/js/jquery.dataTables.min.js"></script>
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/toastr.js/latest/js/toastr.min.js"></script>
    <script src="https://unpkg.com/sweetalert/dist/sweetalert.min.js"></script>
    <script src="https://kit.fontawesome.com/e19c476714.js"></script>
    
  How To call Ajax and display datatable 
  ------------------------------------------------------------------------------------------------------
  var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/nationalParks/GetAllNationalPark",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "name", "width": "50%" },
            { "data": "state", "width": "20%" },
            {
                "data": "id",
                "render": function (data) {
                    return `<div class="text-center">
                                <a href="/nationalParks/Upsert/${data}" class='btn btn-success text-white'
                                    style='cursor:pointer;'> <i class='far fa-edit'></i></a>
                                    &nbsp;
                                <a onclick=Delete("/nationalParks/Delete/${data}") class='btn btn-danger text-white'
                                    style='cursor:pointer;'> <i class='far fa-trash-alt'></i></a>
                                </div>
                            `;
                }, "width": "30%"
            }
        ]
    });
}

function Delete(url) {
    swal({
        title: "Are you sure want to Delete ?",
        text: "You will not be able to resore the data!",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((whileDelete) => {
        if (whileDelete) {
            $.ajax({
                type: 'DELETE',
                url: url,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        dataTable.ajax.reload();
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}

Insert and Display Image 
--------------------------------------------------------------------------
 public byte[] Picture { get; set; }
 
 <form method="post" asp-action="Upsert" enctype="multipart/form-data">
 
 <div class="form-group row">
                <div class="col-4">
                    <label asp-for="Picture"></label>
                </div>
                <div class="col-8">
                    <input type="file" asp-for="Picture" id="projectImage" name="files" multiple class="form-control" />

                </div>
            </div>
     </form>
     
  controller---->
  var files = HttpContext.Request.Form.Files;
                if (files.Count>0)
                {
                    byte[] p1 = null;
                    using (var fs1= files[0].OpenReadStream())
                    {
                        using (var ms1= new MemoryStream())
                        {
                            fs1.CopyTo(ms1);
                            p1 = ms1.ToArray();

                        }
                    }
                    obj.Picture = p1;
                }
                And for display Image 
         <div class="col-4 text-right" style="width:250px">
            @if (Model.Picture != null)
            {
                var base64 = Convert.ToBase64String(Model.Picture);
                var imgsrc = string.Format("data:image/jpg;base64,{0}", base64);

                <img src="@imgsrc" width="100%" />
            }

        </div>
        
        
 DropDownlist:
 ------------------------------------------------------------------------
  public class TrailsVM
    {
        public IEnumerable<SelectListItem> NationalParkList { get; set; }
        public Trail Trails { get; set; }

    } // Here we populate list of national park .
    
 Controller: 
  IEnumerable<NationalPark> npList = await _npRepo.GetAllAsync(SD.NationalParkAPIPath);
            TrailsVM objVM = new TrailsVM()
            {
                NationalParkList = npList.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
                Trails = new Trail()
            };
   View: 
    <div class="form-group row">
                <div class="col-4">
                    National Park
                </div>
                <div class="col-8">
                    @Html.DropDownListFor(m => m.Trails.NationalParkId,
                   Model.NationalParkList, "-Please Select a park-", new { @class = "form-control" })
                    <span asp-validation-for="Trails.NationalParkId" class="text-danger"></span>
                </div>
            </div>
            
            
 DateTime Picker:
 ------------------------------------------------------------------------------------------------
 <div class="form-group row">
                <div class="col-4">
                    <label asp-for="Established"></label>
                </div>
                <div class="col-8">
                    @{ 
                        var dateEst = "";
                        if (Model.Id!=0)
                        {
                            dateEst = Model.Established.ToShortDateString();

                        }
                    }
                    <input id="datepicker" value="@dateEst" type="text" asp-for="@Model.Established"  class="form-control" />
                    
                </div>
            </div>
            @section scripts{ 
   <script>
       $(function () {
           $('#datepicker').datepicker({
               changeMonth: true,
               changeYear: true,
               yearRange: "1500:2020"
           });
       })
   </script>

}
                
