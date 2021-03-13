import React, { useContext } from "react";
import { Table, Image, Icon } from "semantic-ui-react";

import { FolderContext } from "../context/FolderContext";
import DeleteFolderModal from "./DeleteFolderModal";
import EditFolderModal from "./EditFolderModal";
import img from "../assets/foldericon.jpg";
import UploadFolder from "./UploadFolder";

const FolderTable = (props) => {
  const { folder } = useContext(FolderContext);
  return (
    <div>
      <UploadFolder />
      <div style={{ paddingTop: 10 }}>
        <Table selectable>
          <Table.Header>
            <Table.Row>
              <Table.HeaderCell onClick={(e) => {}}>Name</Table.HeaderCell>
              <Table.HeaderCell onClick={(e) => {}}>Size</Table.HeaderCell>
              <Table.HeaderCell onClick={() => {}}>Created At</Table.HeaderCell>
              <Table.HeaderCell>Operations</Table.HeaderCell>
            </Table.Row>
          </Table.Header>
          <Table.Body>
            {folder.folders.map((folder) => (
              <Table.Row
                key={folder.id}
                onClick={() => {
                  console.log("clicked this row " + folder.id);
                }}
              >
                <Table.Cell collapsing>
                  <Image src={img} />
                  {folder.folderName}
                </Table.Cell>
                <Table.Cell>{folder.size}</Table.Cell>
                <Table.Cell>
                  {new Date(folder.createdAt).toLocaleDateString("TR-tr")}-
                  {new Date(folder.createdAt).toLocaleTimeString("TR-tr")}
                </Table.Cell>
                <Table.Cell>
                  <EditFolderModal id={folder.id} name={folder.folderName} />
                  <DeleteFolderModal id={folder.id} />
                  {folder.size === 0 ? null : <Icon name="download" />}
                </Table.Cell>
              </Table.Row>
            ))}
          </Table.Body>
        </Table>
      </div>
    </div>
  );
};

export default FolderTable;
