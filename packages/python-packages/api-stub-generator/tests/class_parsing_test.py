# -------------------------------------------------------------------------
# Copyright (c) Microsoft Corporation. All rights reserved.
# Licensed under the MIT License. See License.txt in the project root for
# license information.
# --------------------------------------------------------------------------

from apistub.nodes import ClassNode

from typing import TypedDict

FakeTypedDict = TypedDict(
    'FakeTypedDict',
    name=str,
    age=int
)


class TestClassParsing:
    
    def test_typed_dict_class(self):
        class_node = ClassNode("test", None, FakeTypedDict, "test")
        assert len(class_node.child_nodes) == 2
        assert class_node.child_nodes[1].name == "name"
        assert class_node.child_nodes[1].type == "str"
        assert class_node.child_nodes[0].name == "age"
        assert class_node.child_nodes[0].type == "int"