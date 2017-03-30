
angular.module('gallery', ['ngRoute'])
    .config([
        '$locationProvider', '$routeProvider',
        function ($locationProvider, $routeProvider) {
            $routeProvider
                /* admin */
                .when('/Angular/gallery', {
                    templateUrl: '/views/Angular/gallery.html',
                    controller: 'GalleryController'
                })
                .when('/Angular/addImg', {
                    templateUrl: '/views/Angular/addImg.html',
                    controller: 'AddImgController'
                })
                .when('/Angular/text', {
                    templateUrl: '/views/Angular/text.html',
                    controller: 'TextController'
                })
                .when('/Angular/About', {
                    templateUrl: '/views/Angular/About.html',
                    controller: 'TextController'
                })
                 .when('/Angular/Album/:albumId',
                 {
                     templateUrl: '/views/Angular/gallery.html',
                     controller: 'AlbumController'
                 })
                     .when('/Angular/:albumId/:imageId',
                 {
                     templateUrl: '/views/Angular/gallery.html',
                     controller: 'ImageController'
                 })
                .otherwise({
                    redirectTo: '/Angular/gallery'
                });

            // Uses HTLM5 history API for navigation
            $locationProvider.html5Mode(true);
        }
    ])
    .controller('TextController', ['$scope', function ($scope) {
        $scope.text = "A minimalistic site dedicated to motorcycle subjects with a photo album and a brief description of the images. Join the contemplation :)";
        $scope.isEdit = false;

        $scope.goEdit = function () {
            $scope.isEdit = true;
        }

        $scope.applyEdit = function () {
            $scope.isEdit = false;
        }
    }])

     .controller('GalleryController', ['$scope', 'dataCenter', function ($scope, dataCenter) {

         $scope.albums = {};
         $scope.currentAlbum = {};
         $scope.currentAlbum.Images = {};
         $scope.currentExtensions = {};


         dataCenter.getAlbumNames().then(function (response) {
             $scope.albums = response.data;
             $scope.currentAlbum = $scope.albums[0];
             $scope.changeAlbum();
         });

         $scope.remove = function (imageId) {
             dataCenter.deleteImage(imageId);
             $scope.currentAlbum.Images = $scope.currentAlbum.Images.filter(i => i.Id !== imageId);
         }

         $scope.changeAlbum = function () {
             dataCenter.getAlbum($scope.currentAlbum.Id).then(function (response) {
                 $scope.currentAlbum.Images = response.data;
             });
         };


         $scope.filterExtension = function (image) {
             // Display the wine if
             var display =
                 // the extensions category checkbox is checked (`filter[category]` is true)
                 $scope.currentExtensions[image.Extension] ||   // or 

                 // no checkbox is checked (all `filter[...]` are false)
                 noFilter($scope.currentExtensions);

             return display;
         };

         function noFilter(filterObj) {
             return Object.
                 keys(filterObj).
                 every(function (key) { return !filterObj[key]; });
         }
     }
     ])



     .controller('AddImgController', ['$scope', 'dataCenter', function ($scope, dataCenter) {

         $scope.imageToAdd = {
             fileName: "",
             description: "",
             data: null,
             albumName: "Sport",
             extension: ""
         };

         $scope.file = document.getElementById('imageToAdd');

         $scope.albums = dataCenter.getAlbumNames;

         $scope.addImg = function () {

             delExtension();

             dataCenter.addImage(
                 $scope.imageToAdd.fileName,
                 $scope.imageToAdd.description,
                 $scope.imageToAdd.data,
                 $scope.imageToAdd.albumName,
                 $scope.imageToAdd.extension
                 );

             $scope.imageToAdd = {
                 fileName: "Done",
                 description: "",
                 data: null,
                 albumName: "",
                 extension: ""
             };

         }

         function delExtension() {

             var a = $scope.imageToAdd.fileName.match(/\.([^\.]+)$/);
             if (a) {
                 $scope.imageToAdd.extension = a[1];
                 $scope.imageToAdd.fileName = $scope.imageToAdd.fileName.replace(/\.[^/.]+$/, "");
             }

         }

         $scope.file.onchange = function (e) {

             var a = this.value.match(/[^\\]+$/)[0];

             $scope.imageToAdd.extension = a.match(/\.([^\.]+)$/)[1];
             $scope.imageToAdd.fileName = a.replace(/\.([^\.]+)$/, "")
             switch ($scope.imageToAdd.extension) {
                 case 'jpeg':
                 case 'jpg':
                 case 'bmp':
                 case 'png':
                 case 'tif':
                     break;
                 default:
                     alert('Incorrect format of image!');
                     this.value = '';
             }
         };

     }])

    .service('dataCenter', ['$http', function ($http) {
        function getAllImages() {
            var response = $http({
                url: '/Image/GetImages'
            });

            return response;
        };

        function getAlbum(selectedAlbumId) {
            var url = '/Image/GetImagesFromAlbum' + '/' + selectedAlbumId;
            var response = $http({
                url: url
            });

            return response;
        };

        function getAlbumNames() {
            var response = $http({
                url: '/Image/GetAlbums'
            });

            return response;
        };

        function addImage(fileName, description, data, albumName, extension) {
            var response = {
                method: "POST",
                url: "/Image/AddImageAjax",
                data: {
                    fileName: fileName,
                    description: description,
                    data: data,
                    albumName: albumName,
                    extension: extension
                },
                headers: { 'Accept': 'application/json' }
            }
            $http(response);

        };

        function deleteImage(imageId) {
            var response = {
                method: 'POST',
                url: '/Image/RemoveImage',
                data: { id: imageId },

                headers: { 'Accept': 'application/json' }
            }

            $http(response);
        };


        return {
            getAllImages: getAllImages,
            getAlbum: getAlbum,
            getAlbumNames: getAlbumNames,
            addImage: addImage,
            deleteImage: deleteImage
        }
    }])
    .directive('dateNow', ['$filter', function ($filter) {
        return {
            link: function ($scope, $element, $attrs) {
                $element.text($filter('date')(new Date(), $attrs.dateNow));
            }
        };
    }])
    .directive("fileread", [function () {
        return {
            scope: {
                fileread: "="
            },
            link: function (scope, element, attributes) {
                element.bind("change", function (changeEvent) {
                    var reader = new FileReader();
                    reader.onload = function (loadEvent) {
                        scope.$apply(function () {
                            scope.fileread = loadEvent.target.result;
                        });
                    }
                    reader.readAsDataURL(changeEvent.target.files[0]);
                });
            }
        }
    }]);;
