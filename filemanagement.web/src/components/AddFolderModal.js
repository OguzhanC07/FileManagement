import React, { useContext, useState } from "react";
import { Button, Modal, Form } from "semantic-ui-react";
import { useTranslation } from "react-i18next";

import { FolderContext, SETFOLDERS } from "../context/FolderContext";
import { getfolders, addfolders } from "../services/folderService";

const AddFolderModal = () => {
  const [open, setOpen] = useState(false);
  const [name, setName] = useState("");
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState(null);
  const { folder, dispatch } = useContext(FolderContext);
  const { t } = useTranslation();

  const addFolderModalHandler = async (e) => {
    setError("");
    e.preventDefault();
    if (name !== "") {
      try {
        setIsLoading(true);
        await addfolders(name, folder.folderId);
        getfolders(folder.folderId).then((res) => {
          dispatch({
            type: SETFOLDERS,
            folders: res.data.data,
          });
          setOpen(false);
        });
        setName("");
        setIsLoading(false);
      } catch (err) {
        if (err.message === "connection")
          setError(t("responseErrors.connection"));
        else if (err.message === "wentWrong")
          setError(t("responseErrors.wentWrong"));
        else setError(err.message);
        setIsLoading(false);
      }
    } else {
      setError(t("addfolderModal.addValidation"));
    }
  };

  return (
    <div>
      <Button
        primary
        onClick={() => {
          setOpen(true);
        }}
      >
        {t("addfolderModal.addFolder")}
      </Button>
      <Modal
        onOpen={() => setOpen(true)}
        closeIcon
        open={open}
        size="small"
        onClose={() => {
          setError("");
          setOpen(false);
        }}
      >
        <h3 style={{ textAlign: "center", paddingBottom: 10 }}>
          {t("addfolderModal.addFolder")}
        </h3>
        <hr />
        <div style={{ margin: 20 }}>
          <Form
            onSubmit={(e) => {
              addFolderModalHandler(e);
            }}
          >
            <Form.Field>
              <label>{t("addfolderModal.folderName")}</label>
              <input
                placeholder={t("addfolderModal.folderName")}
                value={name}
                onChange={(e) => setName(e.target.value)}
              />
            </Form.Field>
            <Button type="submit" loading={isLoading}>
              {t("addfolderModal.submit")}
            </Button>
          </Form>
          <p style={{ textAlign: "center", color: "red" }}>{error}</p>
        </div>
      </Modal>
    </div>
  );
};

export default AddFolderModal;
