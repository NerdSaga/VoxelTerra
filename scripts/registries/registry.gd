@abstract
extends Node
class_name Registry

var prefix := "registry"

var _items: Dictionary[String, RegistryItem] = {}

@abstract func get_item(item_name: String) -> RegistryItem
@abstract func get_by_index(index: int) -> RegistryItem

func _register(script_name: String, item_name: String) -> void:
    var script_dir: String = get_script().resource_path.get_base_dir() + "/blocks/" + script_name
    var item: Script = load(script_dir);

    if item == null:
        push_error("Script " + script_dir + " does not exist")
    
    _items.set(item_name, item.new())

func _index_items() -> void:
    
    _items.sort()
    var index := 0
    for item_name: String in _items:
        var item: RegistryItem = _items[item_name]
        item.id = index
        index += 1


func print_items() -> void:
    for item_name in _items:
        var item: RegistryItem = _items[item_name]
        print(str(item.id) + ": " + prefix + ":" + item_name)