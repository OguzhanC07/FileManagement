import React from "react";
import { Breadcrumb } from "semantic-ui-react";

import "../styles/modal.css";

const BreadcrumbComponent = (props) => {
  return (
    <div className="align breadcrumbDivAlign">
      <Breadcrumb className="breadcrumbFont">
        <Breadcrumb.Section
          onClick={() => {
            props.removeStack(0);
          }}
        >
          /
        </Breadcrumb.Section>
        {props.folderStack.map((folder, i, { length }) =>
          length - 1 === i ? (
            <Breadcrumb.Section id={folder.id} key={folder.id}>
              {folder.folderName}
            </Breadcrumb.Section>
          ) : (
            <Breadcrumb.Section
              onClick={() => {
                props.removeStack(folder.id);
              }}
              key={folder.id}
            >
              {folder.folderName} /
            </Breadcrumb.Section>
          )
        )}
      </Breadcrumb>
    </div>
  );
};

export default BreadcrumbComponent;
