



angular.module('nand2tetris').controller('n2t-fileCtrl', ['$scope', 'sourceFileFactory', 'fileProcessingFactory', function ($scope, sourceFileFactory, fileProcessingFactory) {

    $scope.hi = 'yo';
    $scope.fileExtensionFilter = ".hdl";
    $scope.testCases = [];

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
                
                var temptestCases = file.minFileBody.split(';');
                // remove first element (file loading) and save file names for later
                $scope.testSetupFiles = $scope.extractFileNames(temptestCases.shift());

                // remove last element (blank lines)
                temptestCases.pop();

                angular.forEach(temptestCases, function (value, key) {
                    var testCase = {};
                    testCase.test = value.replace(/^\s+|\s+$/g, "");
                    testCase.result = '';

                    $scope.testCases.push(testCase);
                });
            }
            else
                if ($scope.fileExtensionFilter == ".cmp") {
                    $scope.compareFile = file;
                }
    };

    $scope.selectTestCase = function (testCase) {
        $scope.selectedTestCase = testCase;
    };

    $scope.extractFileNames = function (fileNameText) {
        var filesToLoad = fileNameText.split(',');
        var hdlFile = filesToLoad[0].split(' ')[1];
        var cmpFile = filesToLoad[2].split(' ')[1];
        return { hdl: hdlFile, cmp: cmpFile };
    };

    $scope.ListFiles();

}]);



