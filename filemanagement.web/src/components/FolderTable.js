import React from "react";
import { Table, Icon } from "semantic-ui-react";

const FolderTable = (props) => {
  const deleteHandler = (id) => {
    console.log(id);
  };
  const editHandler = (id) => {
    console.log(id);
  };

  return (
    <Table>
      <Table.Header>
        <Table.Row>
          <Table.HeaderCell onClick={(e) => {}}>Name</Table.HeaderCell>
          <Table.HeaderCell onClick={(e) => {}}>Size</Table.HeaderCell>
          <Table.HeaderCell onClick={() => {}}>Created At</Table.HeaderCell>
          <Table.HeaderCell>Operations</Table.HeaderCell>
        </Table.Row>
      </Table.Header>
      <Table.Body>
        {props.data.map((folder) => (
          <Table.Row
            key={folder.id}
            onClick={() => {
              console.log("clicked this" + folder.id);
            }}
          >
            <Table.Cell>{folder.folderName}</Table.Cell>
            <Table.Cell>{folder.size}</Table.Cell>
            <Table.Cell>{folder.createdAt}</Table.Cell>
            <Table.Cell>
              <Icon
                name="edit"
                onClick={() => {
                  editHandler(folder.id);
                }}
              />
              <Icon
                name="delete"
                onClick={() => {
                  deleteHandler(folder.id);
                }}
              />
            </Table.Cell>
          </Table.Row>
        ))}
      </Table.Body>
    </Table>
  );
};

export default FolderTable;
