using FileManagement.Business.Concrete;
using FileManagement.Business.Interfaces;
using FileManagement.DataAccess;
using FileManagement.DataAccess.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FileManagement.Test
{
    public class FolderServiceTest
    {
        private readonly FolderManager _sut;
        private readonly Mock<IGenericDal<Folder>> _folderRepoMock = new Mock<IGenericDal<Folder>>();
        private readonly Mock<IFolderDal> _folderDalMock = new Mock<IFolderDal>();
        public FolderServiceTest()
        {
            _sut = new FolderManager(_folderDalMock.Object, _folderRepoMock.Object);
        }

        [Fact]
        public async Task GetFolderById_ShouldReturnFolder_WhenFolderExist()
        {
            var random = new Random();
            int folderId = random.Next(1, 100);
            var folderName = "test filename";

            var mockFolder = new Folder
            {
                Id = folderId,
                FolderName = folderName,
                FileGuid = Guid.NewGuid(),
                Size = 500,
                CreatedAt = DateTime.Now,
                IsDeleted = false,
                AppUserId = 1,
            };

            _folderRepoMock.Setup(x => x.GetById(folderId)).ReturnsAsync(mockFolder);

            //Act
            var folder = await _sut.GetById(folderId);

            //Assert
            Assert.Equal(folderId, folder.Id);
            Assert.Equal(folderName, mockFolder.FolderName);
        }

        [Fact]
        public async Task GetFolderById_ShouldReturnNothing_WhenFolderDoesNotExits()
        {
            //Arrange
            var random = new Random();
            int folderId = random.Next(1, 100);
            _folderRepoMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(() => null);

            //Act
            var file = await _sut.GetById(folderId);

            //Assert
            Assert.Null(file);
        }

        [Fact]
        public async Task AddFolders_ShouldReturnNothing_WhenUserExist()
        {
            //Arrange
            var random = new Random();
            int folderId = random.Next(1, 100);
            var folderName = "test filename";

            var mockFolder = new Folder
            {
                Id = folderId,
                FolderName = folderName,
                FileGuid = Guid.NewGuid(),
                Size = 500,
                CreatedAt = DateTime.Now,
                IsDeleted = false,
                AppUserId = 1,
            };

            _folderRepoMock.Setup(x => x.AddAsync(It.IsAny<Folder>())).Verifiable();

            //Act
            await _sut.AddAsync(mockFolder);

            //Assert
            _folderRepoMock.Verify(x => x.AddAsync(It.IsAny<Folder>()), Times.Once());
        }

        [Fact]
        public async Task UpdateFolderById_ShouldReturnNothing_WhenFolderExists()
        {
            //Arrange
            var random = new Random();
            int folderId = random.Next(1, 100);
            var folderName = "test filename";

            var mockFolder = new Folder
            {
                Id = folderId,
                FolderName = folderName,
                FileGuid = Guid.NewGuid(),
                Size = 500,
                CreatedAt = DateTime.Now,
                IsDeleted = false,
                AppUserId = 1,
            };
            _folderRepoMock.Setup(x => x.UpdateAsync(It.IsAny<Folder>())).Verifiable();
            _folderRepoMock.Setup(x => x.AddAsync(It.IsAny<Folder>())).Verifiable();

            //Act
            await _sut.AddAsync(mockFolder);
            mockFolder.FolderName = "test foldername2";
            await _sut.UpdateAsync(mockFolder);

            //Arrange
            _folderRepoMock.Verify(x => x.AddAsync(It.IsAny<Folder>()), Times.Once());
            _folderRepoMock.Verify(x => x.UpdateAsync(It.IsAny<Folder>()), Times.Once());
        }

        [Fact]
        public async Task DeleteFolderById_ShouldReturnNothing_WhenFolderExists()
        {
            //Arrange
            var random = new Random();
            int folderId = random.Next(1, 100);
            var folderName = "test filename";

            var mockFolder = new Folder
            {
                Id = folderId,
                FolderName = folderName,
                FileGuid = Guid.NewGuid(),
                Size = 500,
                CreatedAt = DateTime.Now,
                IsDeleted = false,
                AppUserId = 1,
            };
            _folderRepoMock.Setup(x => x.RemoveAsync(It.IsAny<Folder>())).Verifiable();
            _folderRepoMock.Setup(x => x.AddAsync(It.IsAny<Folder>())).Verifiable();

            //Act
            await _sut.AddAsync(mockFolder);
            await _sut.RemoveAsync(mockFolder);

            //Arrange
            _folderRepoMock.Verify(x => x.AddAsync(It.IsAny<Folder>()), Times.Once());
            _folderRepoMock.Verify(x => x.RemoveAsync(It.IsAny<Folder>()), Times.Once());
        }

        [Fact]
        public async Task GetAllSubFoldersWithFolderId_ShouldReturnListOfFolders_WhenFolderExists()
        {
            //Arrange
            List<Folder> mockData = new List<Folder>{
                new Folder
                {
                    Id = 1,
                    FolderName = "test",
                    CreatedAt = DateTime.Now,
                    Size = 500,
                    FileGuid = Guid.NewGuid(),
                    AppUserId = 1,
                    IsDeleted = false,
                    ParentFolderId = null,
                    InverseParentFolder = new List<Folder>
                    {
                        new Folder{Id=6,FolderName="InverseSub",AppUserId=1,FileGuid=Guid.NewGuid(),CreatedAt=DateTime.Now.AddMinutes(1),ParentFolderId=1,IsDeleted=false,Size=800}
                    }
                }
            };
            _folderDalMock.Setup(x => x.GetAllSubFolders(It.IsAny<int>())).ReturnsAsync(mockData);
            var expected = mockData.Count;
            //Act
            var actual = await _sut.GetAllSubFolders(1);
            //Assert
            Assert.Equal(expected, actual.Count);
        }


        [Fact]
        public async Task GetFoldersByUserId_ShouldReturnListOfFolders_WhenUserExist()
        {
            //Arrange
            var userId = 1;
            var expected = testFolderList.Where(I => I.AppUserId == userId && I.IsDeleted == false && I.ParentFolderId == null).ToList();

            _folderRepoMock.Setup(x => x.GetAllByFilter(It.IsAny<Expression<Func<Folder, bool>>>())).ReturnsAsync((Expression<Func<Folder, bool>> exp) =>
               {
                   return testFolderList.Where(exp.Compile()).ToList();
               });


            //Actual
            var actual = await _sut.GetFoldersByUserId(userId);

            //Assert
            Assert.Equal(expected.Count, actual.Count);

            for (int i = 0; i < expected.Count; i++)
            {
                Assert.Equal(expected[i].FolderName, actual[i].FolderName);
                Assert.Equal(expected[i].Size, actual[i].Size);
            }

        }

        [Fact]
        public async Task GetSubFoldersByFolderId_ShouldReturnListOfFolders_WhenFolderExists()
        {
            //Arrange
            var folderId = 2;
            var expected = testFolderList.Where(I => I.ParentFolderId == folderId && I.IsDeleted == false).ToList();
            _folderRepoMock.Setup(x => x.GetAllByFilter(It.IsAny<Expression<Func<Folder, bool>>>())).ReturnsAsync((Expression<Func<Folder, bool>> exp) =>
            {
                return testFolderList.Where(exp.Compile()).ToList();
            });

            //Act
            var actual = await _sut.GetSubFoldersByFolderId(folderId);


            ////Assert
            Assert.Equal(expected.Count, actual.Count);

            for (int i = 0; i < expected.Count; i++)
            {
                Assert.Equal(expected[i].FolderName, actual[i].FolderName);
            }
        }

        [Fact]
        public async Task FindFolderById_ShouldReturnFolder_WhenFolderExists()
        {
            //Arrange
            var expected = testFolderList.Where(I => I.Id == 1 && I.IsDeleted == false).SingleOrDefault();
            _folderRepoMock.Setup(x => x.GetByFilter(It.IsAny<Expression<Func<Folder, bool>>>())).ReturnsAsync((Expression<Func<Folder, bool>> exp) => 
            {
                return testFolderList.Where(exp.Compile()).SingleOrDefault();
            });
            //Act
            var actual = await _sut.FindFolderById(1);

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(expected,actual);
        }

        private List<Folder> testFolderList= new List<Folder>
            {
                new Folder() { Id=1,AppUserId=1, IsDeleted=false, ParentFolderId=null, FolderName = "a", Size = 1 },
                new Folder() { Id=2, AppUserId=1, IsDeleted=true, ParentFolderId=null, FolderName = "b", Size = 2 },
                new Folder() { Id=3, AppUserId=1, IsDeleted=false, ParentFolderId=2, FolderName = "c", Size = 3 },
                new Folder() { Id=4, AppUserId=2, IsDeleted=false, ParentFolderId=null, FolderName = "a", Size = 4 },
            };
    }
}
