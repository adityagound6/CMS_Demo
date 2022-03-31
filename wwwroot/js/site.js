// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function getResponce(deptId, empId) {
    if (confirm("Are You sure To delete This User?") == true) {
        window.location.replace(`/admin/delete?deptId=${deptId}&empId=${empId}`)
    }
}