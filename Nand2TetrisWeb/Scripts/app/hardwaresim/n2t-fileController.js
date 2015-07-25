



angular.module('nand2tetris').controller('n2t-fileCtrl', ['$scope', '$http', 'sourceFileFactory', 'fileProcessingFactory', function ($scope, $http, sourceFileFactory, fileProcessingFactory) {

    $scope.hi = 'yo';
    $scope.fileExtensionFilter = ".hdl";

    $scope.ListFiles = function () {
        sourceFileFactory.GetSourceFiles()
            .success(function (data, status, headers, config) {
                $scope.sourceFileData = data;
            })
            .error(function (data, status, headers, config) {

            });
    };


    $scope.newFileSubmit = function () {
        sourceFileFactory.NewFile($scope.newFileName)
            .success(function (data, status, headers, config) {
                $scope.newFileName = "";
                $scope.ListFiles();
            })
            .error(function (data, status, headers, config) {
            });

    };

    $scope.saveFile = function () {
        sourceFileFactory.SaveFile($scope.selectedFile.id, $scope.selectedFile.FileName, $scope.selectedFile.FileBody)
            .success(function (data, status, headers, config) {
                $scope.ListFiles();

                // Reselect the file
                angular.forEach($scope.sourceFileData, function (value, index) {
                    if(value.id == $scope.selectedFile.id)
                    {
                        $scope.selectFile(value);
                    }
                })
            })
            .error(function (data, status, headers, config) {

            });
    };


    $scope.parseFile = function () {
        fileProcessingFactory.ParseFile($scope.selectedFile.id)
            .success(function (data, status, headers, config) {
                $scope.parsedFile = data;
            })
            .error(function (data, status, headers, config) {

            });
    };

    $scope.processFile = function () {
        fileProcessingFactory.ProcessChip($scope.selectedFile.id, $scope.parsedFile.inputs, $scope.inputValues)
            .success(function (data, status, headers, config) {
                $scope.outputValues = JSON.parse(data);
                $scope.fileProcessed = true;
            })
            .error(function (data, status, headers, config) {

            });
    };


    $scope.selectFile = function (file) {
        $scope.selectedFile = file;
        $scope.inputValues = [];
        $scope.fileProcessed = false;
        $scope.parseFile();
    };

    $scope.ListFiles();

}]);



