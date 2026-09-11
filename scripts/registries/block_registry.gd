extends Registry


func _enter_tree() -> void:
    self.prefix = "block"
    _register("stone.gd", "stone")
    _register("grass_block.gd", "grass_block")
    _register("sand_block.gd", "sand_block")
    _register("dirt_block.gd", "dirt_block")
    _register("maple_log.gd", "maple_log")
    _index_items()
    print_items()


func get_item(item_name: String) -> BlockRegistryItem:
    return _items.get(item_name) as BlockRegistryItem

func get_by_index(index: int) -> BlockRegistryItem:
    return _items.values()[index] as BlockRegistryItem