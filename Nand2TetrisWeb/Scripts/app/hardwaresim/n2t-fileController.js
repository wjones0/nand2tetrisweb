



angular.module('nand2tetris').controller('n2t-fileCtrl', ['$scope', 'sourceFileFactory', 'fileProcessingFactory', function ($scope, sourceFileFactory, fileProcessingFactory) {

    $scope.hi = 'yo';
    $scope.fileExtensionFilter = ".hdl";

    $scope.ListFiles = function () {
        $scope.filesLoading = true;
        sourceFileFactory.GetSourceFiles()
            .success(function (data, status, headers, config) {
                $scope.sourceFileData = data;
                $scope.filesLoading = false;
            })
            .error(function (data, status, headers, config) {

            });
    };


    $scope.newFileSubmit = function () {
        $scope.filesLoading = true;
        sourceFileFactory.NewFile($scope.newFileName)
            .success(function (data, status, headers, config) {
                $scope.newFileName = "";
                $scope.ListFiles();
            })
            .error(function (data, status, headers, config) {
            });

    };

    $scope.saveFile = function () {
        $scope.fileSaving = true;
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
                $scope.fileSaving = false;
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
        $scope.fileProcessing = true;
        fileProcessingFactory.ProcessChip($scope.selectedFile.id, $scope.parsedFile.inputs, $scope.inputValues)
            .success(function (data, status, headers, config) {
                $scope.outputValues = JSON.parse(data);
                $scope.fileProcessed = true;
                $scope.fileProcessing = false;
            })
            .error(function (data, status, headers, config) {

            });
    };


    $scope.selectFile = function (file) {
        if ($scope.fileExtensionFilter == ".hdl") {
            $scope.selectedFile = file;
            $scope.inputValues = [];
            $scope.fileProcessed = false;
            $scope.parseFile();
        }
        else
            if ($scope.fileExtensionFilter == ".tst") {
                $scope.testFile = file;
                console.log(JSON.stringify(file));
                $scope.testCases = file.minFileBody.split(';');
                // remove first element (file loading)
                $scope.testCases.shift();
                // remove last element (blank lines)
                $scope.testCases.pop();
            }
            else
                if ($scope.fileExtensionFilter == ".cmp") {
                    $scope.compareFile = file;
                }
    };

    $scope.selectTestCase = function (testCase) {
        $scope.selectedTestCase = testCase;
    }

    $scope.ListFiles();

}]);



