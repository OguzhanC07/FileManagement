import React from "react";
import { Table, Image, Loader } from "semantic-ui-react";
import { ContextMenuTrigger } from "react-contextmenu";

import fileimg from "../assets/fileicon.png";
import EditModal from "./EditModal";
import DeleteModal from "./DeleteModal";
import Viewer from "./Viewer";

const TableFileRow = ({ file, type, convertHandler, collect, isDisabled }) => {
  return (
    <ContextMenuTrigger
      renderTag="tr"
      folderId={file.id}
      name={file.fileName}
      type="file"
      id={type}
      holdToDisplay={1000}
      collect={collect}
    >
      <Table.Cell collapsing>
        <Image src={fileimg} />
        {file.fileName.length > 10
          ? file.fileName.substring(0, 20) + "..."
          : file.fileName}
      </Table.Cell>
      <Table.Cell>{convertHandler(file.size)}</Table.Cell>
      <Table.Cell>
        {new Date(file.uploadedAt).toLocaleDateString("TR-tr")}-
        {new Date(file.uploadedAt).toLocaleTimeString("TR-tr")}
      </Table.Cell>
      <Table.Cell id={file.id} className="file">
        <EditModal id={file.id} type="file" name={file.fileName} />
        <DeleteModal id={file.id} type="file" />
        {file.size === 0 ? null : isDisabled ? <Loader active inline /> : null}
        {file.fileName.split(".")[1] === "pdf" && (
          <Viewer type="pdf" id={file.id} />
        )}
        {file.fileName.match(/[^/]+(jpg|png|gif|tif|tiff|bmp|jpeg)$/) && (
          <Viewer type="img" id={file.id} />
        )}
        {file.fileName.split(".")[1] === "mp4" && (
          <Viewer id={file.id} type="video" />
        )}
      </Table.Cell>
    </ContextMenuTrigger>
  );
};

export default TableFileRow;
