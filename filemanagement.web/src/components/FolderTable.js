import React, { useContext, useState } from "react";
import { Table, Image, Icon } from "semantic-ui-react";

import { FolderContext } from "../context/FolderContext";
import DeleteFolderModal from "./DeleteFolderModal";
import EditFolderModal from "./EditFolderModal";
import img from "../assets/foldericon.jpg";
import UploadFolder from "./UploadFolder";
import { downloadfolder } from "../services/folderService";

const FolderTable = (props) => {
  const [isDisabled, setIsDisabled] = useState(false);
  const { folder } = useContext(FolderContext);

  const downloadHandler = async (id) => {
    console.log(id);
    try {
      setIsDisabled(true);
      // const response = await downloadfolder(id);
      // console.log(response);
      downloadfolder(id)
        .then((res) => res.data.blob())
        .then((blob) => {
          const url = window.URL.createObjectURL(new Blob([blob]));
          const link = document.createElement("a");
          link.href = url;
          link.setAttribute("download");
          document.body.appendChild(link);

          link.click();

          link.parentNode.removeChild(link);
        });
      setIsDisabled(false);
    } catch (error) {
      console.log(error.message);
      setIsDisabled(false);
    }
  };

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
                onClick={(e) => {
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
                  {folder.size === 0 ? null : (
                    <Icon
                      disabled={isDisabled}
                      name="download"
                      onClick={(e) => {
                        downloadHandler(folder.id);
                      }}
                    />
                  )}
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
