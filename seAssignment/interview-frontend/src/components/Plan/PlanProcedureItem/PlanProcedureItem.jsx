import React, { useState,useEffect } from "react";
import ReactSelect from "react-select";

const PlanProcedureItem = ({procedure, planId, users, assignedUsers,handleAddUserToProcedure }) => {
    const [selectedUsers, setSelectedUsers] = useState(assignedUsers);
    useEffect(() => {
        if (selectedUsers.length > 0) {
            const userIds = selectedUsers.map(option => option.value);

            const assignUsers = async () => {
                try {
                    await handleAddUserToProcedure(planId, procedure.procedureId, userIds);
                    console.log("Users assigned to plan procedure");
                } catch (error) {
                    console.error("Error assigning users to plan procedure:", error);
                }
            };

            assignUsers();
        }
    }, [selectedUsers]);

    const handleChange = (selectedOptions) => {
        setSelectedUsers(selectedOptions);
    };
    

    return (
        <div className="py-2">
            <div>
                {procedure.procedureTitle}
            </div>

            <ReactSelect
                className="mt-2"
                placeholder="Select User to Assign"
                isMulti={true}
                options={users}
                value={selectedUsers}
                onChange={handleChange}
            />
        </div>
    );
};

export default PlanProcedureItem;
