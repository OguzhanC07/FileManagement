import React, { useContext, useState } from "react";
import { Table, Image, Icon, Loader } from "semantic-ui-react";
import uuid from "uuid/dist/v1";
import { ToastContainer, toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import { useTranslation } from "react-i18next";

import { FolderContext, SORTFOLDERS } from "../context/FolderContext";
import img from "../assets/foldericon.jpg";
import fileimg from "../assets/fileicon.png";
import { downloadfolder } from "../services/folderService";
import { getsinglefile } from "../services/fileService";
import { FileContext, SORTFILE } from "../context/FileContext";
import EditModal from "./EditModal";
import DeleteModal from "./DeleteModal";
import Viewer from "./Viewer";

const FolderTable = (props) => {
  const [isDisabled, setIsDisabled] = useState(false);
  const [sortType, setSortType] = useState("asc");
  const { folder, dispatch } = useContext(FolderContext);
  const { file, dispatch: fileDispatch } = useContext(FileContext);
  const { t } = useTranslation();

  const downloadHandler = async (id, type) => {
    let response = "";
    let problem = "";
    switch (type) {
      case "file":
        try {
          setIsDisabled(true);
          response = await getsinglefile(id);
          setIsDisabled(false);
        } catch (responseError) {
          problem = responseError;
          setIsDisabled(false);
        }
        break;
      case "folder":
        try {
          setIsDisabled(true);
          response = await downloadfolder(id);
          setIsDisabled(false);
        } catch (responseError) {
          problem = responseError;
          setIsDisabled(false);
        }
        break;
      default:
        break;
    }
    if (typeof problem === "undefined" || problem !== "") {
      toast.error(
        t("folderTable.downloadError") + t("responseErrors.wentWrong")
      );
    } else {
      setIsDisabled(true);
      var data = new Blob([response.data], { type: response.data.type });
      var folderData = window.URL.createObjectURL(data);
      const tempLink = document.createElement("a");
      tempLink.href = folderData;
      tempLink.setAttribute("download", uuid());
      tempLink.click();
      setIsDisabled(false);
    }
  };

  const convertHandler = (bytes) => {
    var sizes = [t("folderTable.bytes"), "KB", "MB", "GB", "TB"];
    if (bytes === 0) return "0 Byte";
    var i = parseInt(Math.floor(Math.log(bytes) / Math.log(1024)));
    return Math.round(bytes / Math.pow(1024, i), 2) + " " + sizes[i];
  };

  const sortingHandler = (type) => {
    sortType === "asc" ? setSortType("desc") : setSortType("asc");

    switch (type) {
      case "name":
        dispatch({
          type: SORTFOLDERS,
          sortType: sortType,
          sortName: "folderName",
        });
        fileDispatch({
          type: SORTFILE,
          sortType: sortType,
          sortName: "fileName",
        });
        break;
      case "size":
        dispatch({
          type: SORTFOLDERS,
          sortType: sortType,
          sortName: "size",
        });
        fileDispatch({
          type: SORTFILE,
          sortType: sortType,
          sortName: "size",
        });
        break;
      default:
        break;
    }
  };

  return (
    <div>
      <div>
        <Table style={{ paddingTop: 10 }} selectable>
          <Table.Header>
            <Table.Row>
              <Table.HeaderCell
                onClick={() => {
                  sortingHandler("name");
                }}
              >
                {t("folderTable.tableHeaderName")}
              </Table.HeaderCell>
              <Table.HeaderCell
                onClick={() => {
                  sortingHandler("size");
                }}
              >
                {t("folderTable.tableHeaderSize")}
              </Table.HeaderCell>
              <Table.HeaderCell>
                {t("folderTable.tableHeaderCreatedAt")}
              </Table.HeaderCell>
              <Table.HeaderCell>
                {t("folderTable.tableHeaderOperations")}
              </Table.HeaderCell>
            </Table.Row>
          </Table.Header>
          <Table.Body>
            {folder.folders.map((folder) => (
              <Table.Row
                key={folder.id}
                id={folder.id.toString()}
                onDoubleClick={() => {
                  props.openFolder(folder.id, folder.folderName);
                }}
              >
                <Table.Cell collapsing>
                  <Image src={img} />
                  {folder.folderName.substring(0, 10)}
                  {folder.folderName.length > 10 ? "..." : null}
                </Table.Cell>
                <Table.Cell>{convertHandler(folder.size)}</Table.Cell>
                <Table.Cell>
                  {new Date(folder.createdAt).toLocaleDateString("TR-tr")}-
                  {new Date(folder.createdAt).toLocaleTimeString("TR-tr")}
                </Table.Cell>
                <Table.Cell>
                  <EditModal
                    id={folder.id}
                    name={folder.folderName}
                    type="folder"
                  />
                  <DeleteModal id={folder.id} type="folder" />
                  {folder.size === 0 ? null : isDisabled ? (
                    <Loader active inline />
                  ) : (
                    <Icon
                      disabled={isDisabled}
                      name="download"
                      onClick={(e) => {
                        downloadHandler(folder.id, "folder");
                      }}
                    />
                  )}
                </Table.Cell>
              </Table.Row>
            ))}

            {file.files.length > 0
              ? file.files.map((file) => (
                  <Table.Row className="file" id={file.id} key={file.id}>
                    <Table.Cell collapsing>
                      <Image src={fileimg} />
                      {file.fileName}
                    </Table.Cell>
                    <Table.Cell>{convertHandler(file.size)}</Table.Cell>
                    <Table.Cell>
                      {new Date(file.uploadedAt).toLocaleDateString("TR-tr")}-
                      {new Date(file.uploadedAt).toLocaleTimeString("TR-tr")}
                    </Table.Cell>
                    <Table.Cell>
                      <EditModal
                        id={file.id}
                        type="file"
                        name={file.fileName}
                      />
                      <DeleteModal id={file.id} type="file" />
                      {file.size === 0 ? null : isDisabled ? (
                        <Loader active inline />
                      ) : (
                        <Icon
                          name="download"
                          onClick={(e) => {
                            downloadHandler(file.id, "file");
                          }}
                        />
                      )}
                      {file.fileName.split(".")[1] === "pdf" && (
                        <Viewer type="pdf" id={file.id} />
                      )}
                      {file.fileName.match(
                        /[^/]+(jpg|png|gif|tif|tiff|bmp|jpeg)$/
                      ) && <Viewer type="img" id={file.id} />}
                      {file.fileName.split(".")[1] === "mp4" && (
                        <Viewer id={file.id} type="video" />
                      )}
                    </Table.Cell>
                  </Table.Row>
                ))
              : null}
          </Table.Body>
        </Table>
        <ToastContainer
          position="top-right"
          autoClose={5000}
          hideProgressBar={false}
          newestOnTop={false}
          closeOnClick
          rtl={false}
          pauseOnFocusLoss
          draggable
          pauseOnHover
        />
      </div>
    </div>
  );
};

export default FolderTable;
