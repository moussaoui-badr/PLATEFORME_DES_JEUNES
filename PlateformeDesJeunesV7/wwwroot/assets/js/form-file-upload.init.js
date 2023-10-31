var onDragEnter = function (event) {
    $(".br_dropzone").addClass("dragover");
},
 
onDragOver = function (event) {
    event.preventDefault();
    if (!$(".br_dropzone").hasClass("dragover"))
        $(".br_dropzone").addClass("dragover");
},
 
onDragLeave = function (event) {
    event.preventDefault();
    $(".br_dropzone").removeClass("dragover");
},
 
onDrop = function (event) {
    $(".br_dropzone").removeClass("dragover");
    $(".br_dropzone").addClass("dragdrop");
    console.log(event.originalEvent.dataTransfer.files);
};
 
$(".br_dropzone")
.on("dragenter", onDragEnter)
.on("dragover", onDragOver)
.on("dragleave", onDragLeave)
.on("drop", onDrop);