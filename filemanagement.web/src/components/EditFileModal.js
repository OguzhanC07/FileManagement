import React, { useContext, useState } from "react";
import { Icon, Modal, Form, Button } from "semantic-ui-react";

import { FileContext, EDITFILE } from "../context/FileContext";
import { editfile } from "../services/fileService";

const EditFileModal = (props) => {
  const [open, setOpen] = useState(false);
  const [name, setName] = useState(props.name.split(".")[0]);
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState(null);

  const { dispatch } = useContext(FileContext);

  const editHandler = async () => {
    setError("");
    if (name !== "") {
      try {
        setIsLoading(true);
        const response = await editfile(props.id, name);
        if (response.status < 400) {
          dispatch({
            type: EDITFILE,
            fid: props.id,
            name: name + "." + props.name.split(".")[1],
          });
        } else {
          setError(response.data.message);
        }
        setIsLoading(false);
        setOpen(false);
      } catch (error) {
        setError(error.message);
        setIsLoading(false);
      }
    } else {
      setError("File name can't be empty");
    }
  };

  return (
    <div>
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
        onClose={() => {
          setOpen(false);
        }}
      >
        <h3 style={{ textAlign: "center", paddingBottom: 10 }}>
          Edit File Name
        </h3>
        <hr />
        <div style={{ margin: 20 }}>
          <Form
            onSubmit={() => {
              editHandler();
            }}
          >
            <Form.Field>
              <label>File Name</label>
              <input
                placeholder="File Name"
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
    </div>
  );
};

export default EditFileModal;
