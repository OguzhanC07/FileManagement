import React from "react";
import { ContextMenuTrigger } from "react-contextmenu";
import { Table, Image, Loader } from "semantic-ui-react";

import img from "../assets/foldericon.jpg";
import EditModal from "./EditModal";
import DeleteModal from "./DeleteModal";

const TableFolderRow = ({
  convertHandler,
  handleDoubleClick,
  collect,
  type,
  folder,
  isDisabled,
}) => {
  return (
    <ContextMenuTrigger
      renderTag="tr"
      folderId={folder.id}
      name={folder.folderName}
      type="folder"
      id={type}
      holdToDisplay={1000}
      collect={collect}
    >
      <Table.Cell
        onDoubleClick={() => {
          handleDoubleClick(folder.id, folder.folderName);
        }}
        collapsing
      >
        <Image src={img} />
        {folder.folderName.substring(0, 10)}
        {folder.folderName.length > 10 ? "..." : null}
      </Table.Cell>
      <Table.Cell
        onDoubleClick={() => {
          handleDoubleClick(folder.id, folder.folderName);
        }}
      >
        {convertHandler(folder.size)}
      </Table.Cell>
      <Table.Cell
        onDoubleClick={() => {
          handleDoubleClick(folder.id, folder.folderName);
        }}
      >
        {new Date(folder.createdAt).toLocaleDateString("TR-tr")}-
        {new Date(folder.createdAt).toLocaleTimeString("TR-tr")}
      </Table.Cell>
      <Table.Cell
        onDoubleClick={() => {
          handleDoubleClick(folder.id, folder.folderName);
        }}
        id={folder.id}
        className="folder"
      >
        <EditModal id={folder.id} name={folder.folderName} type="folder" />
        <DeleteModal id={folder.id} type="folder" />
        {folder.size === 0 ? null : isDisabled ? (
          <Loader active inline />
        ) : null}
      </Table.Cell>
    </ContextMenuTrigger>
  );
};

export default TableFolderRow;
