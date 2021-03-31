import React, { useContext, useState } from "react";
import { Icon, Modal, Button } from "semantic-ui-react";
import { useTranslation } from "react-i18next";

import { DELETEFOLDER, FolderContext } from "../context/FolderContext";
import { DELETEFILE, FileContext } from "../context/FileContext";
import { deletefolder } from "../services/folderService";
import { deletefile } from "../services/fileService";
import "../styles/modal.css";

const DeleteModal = (props) => {
  const [open, setOpen] = useState(false);
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState(null);
  const { dispatch } = useContext(FolderContext);
  const { dispatch: fileDispatch } = useContext(FileContext);
  const { t } = useTranslation();

  const deleteHandler = async (e) => {
    e.preventDefault();
    setError("");
    switch (props.type) {
      case "folder":
        try {
          setIsLoading(true);
          await deletefolder(props.id);
          setIsLoading(false);
          setOpen(false);
          dispatch({
            type: DELETEFOLDER,
            fid: props.id,
          });
        } catch (error) {
          if (error.message === "connection")
            setError(t("responseErrors.connection"));
          else if (error.message === "wentWrong")
            setError(t("responseErrors.wentWrong"));
          else setError(error.message);
          setIsLoading(false);
        }
        break;
      case "file":
        try {
          setIsLoading(true);
          await deletefile(props.id);
          setIsLoading(false);
          setOpen(false);
          fileDispatch({
            type: DELETEFILE,
            fid: props.id,
          });
        } catch (error) {
          if (error.message === "connection")
            setError(t("responseErrors.connection"));
          else if (error.message === "wentWrong")
            setError(t("responseErrors.wentWrong"));
          else setError(error.message);
          setIsLoading(false);
        }
        break;
      default:
        break;
    }
  };

  return (
    <span>
      <Icon
        name="delete"
        onClick={() => {
          setError("");
          setOpen(true);
        }}
      />
      <Modal
        onOpen={() => setOpen(true)}
        basic
        closeIcon
        open={open}
        size="small"
        onClose={() => {
          setOpen(false);
        }}
      >
        <h3 className="header">
          {t("deleteModal.deleteHeader", {
            variable:
              props.type === "file"
                ? t("deleteModal.file")
                : t("deleteModal.folder"),
          })}
        </h3>

        <div className="header">
          <p>
            {t("deleteModal.deleteValidation", {
              variable:
                props.type === "file"
                  ? t("deleteModal.file")
                  : t("deleteModal.folder"),
            })}
          </p>
          <Button
            className="cancelButton"
            basic
            color="red"
            inverted
            onClick={() => setOpen(false)}
          >
            <Icon name="remove" /> {t("deleteModal.noBtn")}
          </Button>
          <Button
            color="green"
            loading={isLoading}
            inverted
            onClick={(e) => deleteHandler(e)}
          >
            <Icon name="checkmark" /> {t("deleteModal.yesBtn")}
          </Button>
        </div>
        <div>{error && <p className="errorMessage">{error}</p>}</div>
      </Modal>
    </span>
  );
};

export default DeleteModal;
