import React, { useContext, useState } from "react";
import { Icon, Modal, Form, Button } from "semantic-ui-react";

import { EDITFILE, FileContext } from "../context/FileContext";
import { EDITFOLDER, FolderContext } from "../context/FolderContext";
import { editfile } from "../services/fileService";
import { editfolder } from "../services/folderService";

const EditModal = (props) => {
  const [open, setOpen] = useState(false);
  const [name, setName] = useState(props.name.split(".")[0]);
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState(null);

  const { dispatch } = useContext(FolderContext);
  const { dispatch: fileDispatch } = useContext(FileContext);

  const editHandler = async (e) => {
    e.preventDefault();
    setError("");
    if (name !== "") {
      let response;
      switch (props.type) {
        case "folder":
          try {
            setIsLoading(true);
            response = await editfolder(props.id, name);
            setIsLoading(false);
            if (response.status < 400) {
              dispatch({
                type: EDITFOLDER,
                fid: props.id,
                name: name,
              });
              setOpen(false);
            } else {
              setError(response.data.message);
            }
            setIsLoading(false);
          } catch (error) {
            setError(error.message);
            setIsLoading(false);
          }
          break;
        case "file":
          try {
            setIsLoading(true);
            response = await editfile(props.id, name);
            if (response.status < 400) {
              fileDispatch({
                type: EDITFILE,
                fid: props.id,
                name: name + "." + props.name.split(".")[1],
              });
              setOpen(false);
            } else {
              setError(response.data.message);
            }
            setIsLoading(false);
          } catch (error) {
            setError(error.message);
            setIsLoading(false);
          }
          break;
        default:
          break;
      }
    } else {
      setError("Name cannot be empty");
    }
  };

  return (
    <span>
      <Icon
        name="edit"
        onClick={() => {
          setOpen(true);
        }}
      />

      <Modal
        onOpen={() => setOpen(true)}
        closeIcon
        open={open}
        size="small"
        onClose={() => {
          setOpen(false);
        }}
      >
        <h3 style={{ textAlign: "center", paddingBottom: 10 }}>
          Edit {props.type === "file" ? "File" : "Folder"} Name
        </h3>
        <hr />
        <div style={{ margin: 20 }}>
          <Form
            onSubmit={(e) => {
              editHandler(e);
            }}
          >
            <Form.Field>
              <label>{props.type === "file" ? "File" : "Folder"} Name</label>
              <input
                placeholder={
                  (props.type === "file" ? "File" : "Folder", " Name")
                }
                value={name}
                onChange={(e) => setName(e.target.value)}
                required
              />
            </Form.Field>
            <Button type="submit" loading={isLoading}>
              Submit
            </Button>
          </Form>
          <p style={{ textAlign: "center", color: "red" }}>{error}</p>
        </div>
      </Modal>
    </span>
  );
};

export default EditModal;
