import React from "react";
import "../styles/admin.css";

const MainContent = (props) => {
  return (
    <main>
      <aside className="right-side">
        <div className="text-con">
          <div>{props.children}</div>
        </div>
      </aside>
    </main>
  );
};

export default MainContent;
