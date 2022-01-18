# -------------------------------------------------------------------------
# Copyright (c) Microsoft Corporation. All rights reserved.
# Licensed under the MIT License. See License.txt in the project root for
# license information.
# --------------------------------------------------------------------------

from dataclasses import dataclass
from apistub.nodes import ClassNode

class FakeInventoryItem:
    """ Class for testing """
    name: str
    unit_price: float
    quantity_on_hand: int = 0

    def total_cost(self) -> float:
        return self.unit_price * self.quantity_on_hand

@dataclass
class FakeInventoryItemDataClass:
    """ Class for testing @dataclass """
    name: str
    unit_price: float
    quantity_on_hand: int = 0

    def total_cost(self) -> float:
        return self.unit_price * self.quantity_on_hand


class TestClassParsing:

    def test_regular_class(self):
        class_node = ClassNode("test", None, FakeInventoryItem, "test")
        assert len(class_node.child_nodes) == 4    

    def test_data_class(self):
        class_node = ClassNode("test", None, FakeInventoryItemDataClass, "test")
        assert len(class_node.child_nodes) == 4