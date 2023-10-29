# GybeCase

**Important Note:** The game has been tested with an Android device up to this [commit](https://github.com/emreavan/GybeCase/commit/a8d07e1778ca3372f03ee293404dfa7c15ff1c13). [Click Here](https://drive.google.com/file/d/12_YUrbZjY_3zatjZOvnSSZ0-n5bJNgQk/view?usp=share_link) to download the APK. Any subsequent changes have not been tested on Android due to the unavailability of a device.

## Features

- I've decided to not use Singleton pattern for `PlayerData` and managers. For this reason I've used `Dependency Injection` and create multiple prefabs for injection. Other than that between `OrderManager` and `OrderUI` and between `ProductManager` and `Plant`, `Observer Pattern` is used.

- Crops are not a part of plants and retrieved from the pool at plant generation. It is possible to add more spawn positions at the plants and if there are more crops than the spawn positions at the plant, other crops are set not active. After crops are ready to be harvest, a particle effect is played.

<img src="https://drive.google.com/uc?export=download&id=16SDxSdNRBLnZBdMTb5v0fB75f_5XdL5T" width="200">

- For a simple ground it might be a overkill but for future proof, I've used `NavMeshSurface` and `NavMeshAgent` for Character movement. Every time ground is scaled, `NavMeshSurface` is also baked once again. Only `Ground` layer is taken into consideration while `NavMeshSurface` is baked.

<img src="https://drive.google.com/uc?export=download&id=1HfowR6W68eEQMbhXIOuXOpQ1JqO3z5sh" width="200">

- To make it look like `Pocket Land`, I've decided to add scaling to the game. It is possible to change the scaling size at `GroundController` script of the `Ground` prefab

<img src="https://drive.google.com/uc?export=download&id=1GxPLchO0wid3EM5xiQ0gniipflzqRGmv" width="200">

- Again to make it look like `Pocket Land`, After 5th level, an order will consist 2 pieces.
  
<img src="https://drive.google.com/uc?export=download&id=1toLUJg3jvKNe6dpxJxV0rWeVxKPc24qv" width="200">

- To make it look more compact, gold coin amount is format accordingly (`K` for `1 000`, `M` for `1 000 000` ...)

<img src="https://drive.google.com/uc?export=download&id=1lwFz-BJSkqrR88FpMURGrpwV-sgX0Pu6" width="200">

## Scriptable Objects

- **Extensibility**: With the use of Scriptable Objects for data storage, adding new Crops and Plants is simplified. The [ItemClass](https://github.com/emreavan/GybeCase/blob/main/Assets/Scripts/ScriptableObjects/ItemClassSO.cs) defines every living object in the scene.
  
- **Pooling**: For efficient memory usage, plants and crops are managed separately, reducing the total count of pooled objects.

### Crops Data

This serialized dictionary contains the `ItemClass` of a Crop along with its associated data:

<img src="https://drive.google.com/uc?export=download&id=1LuNKKvzpCfAJmOUxNWHUcx7Ra_6yRuAQ" width="500">

### Plant Data

This serialized dictionary contains the `ItemClass` of a Plant along with its associated data:

<img src="https://drive.google.com/uc?export=download&id=1DgkLjPoAslAo350WaQsc9vhpNXvBbJEM" width="500">

### CropSO & PlantSO

The `Crop` Scriptable Object enables easy creation of a new crop by adding a GameObject, Sprite, and its Plant Item Class while specifying other constants. `How Many Product` specifies how many crop should be retrieved from the pool for a single plant and `Maximum Product Count` defines in a given time there could be given amount of crop in the scene:

<img src="https://drive.google.com/uc?export=download&id=11jfqY0CzIuwUy_Cj5nUHut9EaofjWTVW" width="500">

The `Plant` Scriptable Object allows for easy addition or modification of plant information.

<img src="https://drive.google.com/uc?export=download&id=1eD-4TrvhIM8PqYFHNP8G3oSRLatHEoPV" width="500">

`Pooled Count` is a public variable in order to let devs to make some changes for performance concerns.

## Version

- Unity Version: 2022.3.5.f1 LTS

## Notes

- For serializable dictionary, I've used [this](https://github.com/EduardMalkhasyan/Serializable-Dictionary-Unity)
