import React, { useContext, useState } from "react";
import { Table, Image, Icon } from "semantic-ui-react";
import uuid from "uuid/dist/v1";

import { FolderContext } from "../context/FolderContext";
import DeleteFolderModal from "./DeleteFolderModal";
import EditFolderModal from "./EditFolderModal";
import img from "../assets/foldericon.jpg";
import fileimg from "../assets/fileicon.png";
import { downloadfolder } from "../services/folderService";
import { getsinglefile } from "../services/fileService";
import { FileContext } from "../context/FileContext";
import EditFileModal from "./EditFileModal";
import DeleteFileModal from "./DeleteFileModal";
import PdfViewer from "./PdfViewer";
import ImgViewer from "./ImgViewer";
import VideoViewer from "./VideoViewer";

const FolderTable = (props) => {
  const [isDisabled, setIsDisabled] = useState(false);

  const { folder } = useContext(FolderContext);
  const { file } = useContext(FileContext);

  const downloadHandler = async (id, type) => {
    let response = "";
    switch (type) {
      case "file":
        try {
          setIsDisabled(true);
          response = await getsinglefile(id);
          setIsDisabled(false);
        } catch (error) {
          console.log(error.message);
          setIsDisabled(false);
        }
        break;
      case "folder":
        try {
          setIsDisabled(true);
          response = await downloadfolder(id);
          setIsDisabled(false);
        } catch (error) {
          console.log(error.message);
          setIsDisabled(false);
        }
        break;

      default:
        break;
    }
    setIsDisabled(true);
    var data = new Blob([response.data], { type: response.data.type });
    var folderData = window.URL.createObjectURL(data);
    const tempLink = document.createElement("a");
    tempLink.href = folderData;
    tempLink.setAttribute("download", uuid());
    tempLink.click();
    setIsDisabled(false);
  };

  const convertHandler = (bytes) => {
    var sizes = ["Bytes", "KB", "MB", "GB", "TB"];
    if (bytes === 0) return "0 Byte";
    var i = parseInt(Math.floor(Math.log(bytes) / Math.log(1024)));
    return Math.round(bytes / Math.pow(1024, i), 2) + " " + sizes[i];
  };

  return (
    <div>
      <div>
        <Table style={{ paddingTop: 10 }} selectable>
          <Table.Header>
            <Table.Row>
              <Table.HeaderCell>Name</Table.HeaderCell>
              <Table.HeaderCell>Size</Table.HeaderCell>
              <Table.HeaderCell>Created At</Table.HeaderCell>
              <Table.HeaderCell>Operations</Table.HeaderCell>
            </Table.Row>
          </Table.Header>
          <Table.Body>
            {folder.folders.map((folder) => (
              <Table.Row
                key={folder.id}
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
                  <EditFolderModal id={folder.id} name={folder.folderName} />
                  <DeleteFolderModal id={folder.id} />
                  {folder.size === 0 ? null : (
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
                  <Table.Row key={file.id}>
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
                      <EditFileModal id={file.id} name={file.fileName} />
                      <DeleteFileModal id={file.id} />
                      {file.size === 0 ? null : (
                        <Icon
                          disabled={isDisabled}
                          name="download"
                          onClick={(e) => {
                            downloadHandler(file.id, "file");
                          }}
                        />
                      )}
                      {file.fileName.split(".")[1] === "pdf" ? (
                        <PdfViewer id={file.id} />
                      ) : null}
                      {file.fileName.match(
                        /[^/]+(jpg|png|gif|tif|tiff|bmp|jpeg)$/
                      ) ? (
                        <ImgViewer id={file.id} />
                      ) : null}
                      {file.fileName.split(".")[1] === "mp4" ? (
                        <VideoViewer id={file.id} />
                      ) : null}
                    </Table.Cell>
                  </Table.Row>
                ))
              : null}
          </Table.Body>
        </Table>
      </div>
    </div>
  );
};

export default FolderTable;
