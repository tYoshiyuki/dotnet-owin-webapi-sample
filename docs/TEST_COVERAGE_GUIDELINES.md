# テストカバレッジ向上のための設計指針

## 1. 概要

本ドキュメントは、`DotNetOwinWebApiSample.Api.Test` プロジェクトのカバレッジ率向上のために、不足しているテストケースを体系的に追加するための設計指針を提供します。

## 2. テスト戦略

### 2.1 テストの種類

プロジェクトでは以下の2種類のテストを実装します：

1. **ユニットテスト（Unit Tests）**
   - 個別のクラス・メソッドを独立してテスト
   - モックを使用して依存関係を分離
   - テストカテゴリ: `[TestCategory("Logic")]`

2. **統合テスト（Integration Tests）**
   - コントローラー全体を通じたエンドツーエンドテスト
   - 実際のHTTPリクエスト/レスポンスを検証
   - テストカテゴリ: `[TestCategory("Integration")]`

### 2.2 テストカバレッジ目標

- **行カバレッジ**: 80%以上
- **分岐カバレッジ**: 75%以上
- **メソッドカバレッジ**: 90%以上

## 3. 不足しているテストケース一覧

### 3.1 Controllers

#### 3.1.1 TodoController

**現状**: `Get()` と `Get(int id)` のみテスト済み

**不足しているテストケース**:

1. **Post メソッド**
   - ✅ 正常系: 有効なToDoPostRequestで更新が成功
   - ✅ 異常系: nullのToDoPostRequest
   - ✅ 異常系: 存在しないIDで更新（ApiNotFoundException）
   - ✅ 異常系: descriptionがnull（ApiBadRequestException）

2. **Put メソッド**
   - ✅ 正常系: 有効なToDoPutRequestで新規追加が成功
   - ✅ 異常系: nullのToDoPutRequest
   - ✅ 異常系: descriptionがnull（ApiBadRequestException）

3. **Delete メソッド**
   - ✅ 正常系: 存在するIDで削除が成功
   - ✅ 異常系: 存在しないIDで削除（ApiNotFoundException）

4. **Get(int id) メソッドの拡張**
   - ✅ 異常系: 存在しないIDで取得（ApiNotFoundException）

#### 3.1.2 GreetingController

**現状**: テストが存在しない

**必要なテストケース**:

1. **Get() メソッド**
   - ✅ 正常系: "Hello"が返される

2. **Get(int hour) メソッド**
   - ✅ 正常系: hour < 5 の場合 "Good evening"が返される
   - ✅ 正常系: 5 <= hour < 12 の場合 "Good morning"が返される
   - ✅ 正常系: 12 <= hour < 18 の場合 "Good afternoon"が返される
   - ✅ 正常系: 18 <= hour < 24 の場合 "Good evening"が返される
   - ✅ 異常系: hour >= 24 の場合 ApiBadRequestExceptionが発生
   - ✅ 異常系: hour < 0 の場合（バリデーションが必要な場合）

### 3.2 Services

#### 3.2.1 TodoService

**現状**: `Get()`, `Get(int id)`, `Update(Todo)` のみテスト済み

**不足しているテストケース**:

1. **Add メソッド**
   - ✅ 正常系: 有効なdescriptionで追加が成功
   - ✅ 異常系: descriptionがnull（ApiBadRequestException）
   - ✅ 異常系: descriptionが空文字列（必要に応じて）

2. **Update(int id, string description) メソッド**
   - ✅ 正常系: 有効なIDとdescriptionで更新が成功
   - ✅ 異常系: 存在しないIDで更新（ApiNotFoundException）
   - ✅ 異常系: descriptionがnull（ApiBadRequestException）

3. **Update(Todo) メソッドの拡張**
   - ✅ 異常系: todoがnull（ApiBadRequestException）

4. **Remove メソッド**
   - ✅ 正常系: 存在するIDで削除が成功
   - ✅ 異常系: 存在しないIDで削除（ApiNotFoundException）

#### 3.2.2 GreetingService

**現状**: `Greeting()` のみテスト済み

**不足しているテストケース**:

1. **Greeting(int hour) メソッド**
   - ✅ 正常系: hour < 5 の場合 "Good evening"
   - ✅ 正常系: 5 <= hour < 12 の場合 "Good morning"
   - ✅ 正常系: 12 <= hour < 18 の場合 "Good afternoon"
   - ✅ 正常系: 18 <= hour < 24 の場合 "Good evening"
   - ✅ 境界値テスト: hour = 0, 4, 5, 11, 12, 17, 18, 23
   - ✅ 異常系: hour >= 24（ApiBadRequestException）
   - ✅ 異常系: hour < 0（必要に応じて）

### 3.3 Repositories

#### 3.3.1 TodoRepository

**現状**: テストが存在しない

**必要なテストケース**:

1. **Get メソッド**
   - ✅ 正常系: 初期データが取得できる

2. **Add メソッド**
   - ✅ 正常系: 新しいTodoが追加される
   - ✅ 異常系: 既に存在するIDのTodoを追加しようとした場合（追加されない）

3. **Update メソッド**
   - ✅ 正常系: 存在するTodoの更新が成功
   - ✅ 異常系: 存在しないIDのTodoを更新しようとした場合（何も起こらない）

4. **Remove メソッド**
   - ✅ 正常系: 存在するTodoの削除が成功
   - ✅ 異常系: 存在しないIDのTodoを削除しようとした場合（エラーが発生しないことを確認）

### 3.4 Middlewares

#### 3.4.1 ErrorHandlingMiddleware

**現状**: 一般的な例外処理のみテスト済み

**不足しているテストケース**:

1. **ApiBadRequestException の処理**
   - ✅ HTTPステータスコードが400（BadRequest）になる
   - ✅ エラーメッセージが正しく返される

2. **ApiNotFoundException の処理**
   - ✅ HTTPステータスコードが404（NotFound）になる
   - ✅ エラーメッセージが正しく返される

3. **その他の例外の処理**
   - ✅ 一般的なExceptionの場合、HTTPステータスコードが500（InternalServerError）になる
   - ✅ エラーメッセージが正しく返される

4. **正常系の処理**
   - ✅ 例外が発生しない場合、正常に処理が継続される

### 3.5 ExceptionHandlers

#### 3.5.1 PassthroughExceptionHandler

**現状**: テストが存在しない

**必要なテストケース**:

1. **HandleAsync メソッド**
   - ✅ 例外が再スローされることを確認
   - ✅ 元の例外情報が保持されることを確認

### 3.6 ControllerActivators

#### 3.6.1 ServiceProviderControllerActivator

**現状**: テストが存在しない

**必要なテストケース**:

1. **Create メソッド**
   - ✅ 正常系: コントローラーが正しく作成される
   - ✅ 正常系: スコープが正しく管理される
   - ✅ 正常系: リクエストの破棄時にスコープが破棄される

### 3.7 Extensions

#### 3.7.1 ServiceProviderExtensions

**現状**: テストが存在しない

**必要なテストケース**:

1. **AddControllersAsServices メソッド**
   - ✅ 正常系: コントローラーが正しく登録される
   - ✅ 正常系: 抽象クラスは除外される
   - ✅ 正常系: ジェネリック型定義は除外される
   - ✅ 正常系: Controllerで終わるクラスのみが登録される

### 3.8 Exceptions

**現状**: テストが存在しない（ただし、例外クラス自体はシンプルなため、優先度は低い）

**必要なテストケース**:

1. **ApiSampleException**
   - ✅ パラメータなしコンストラクタ
   - ✅ メッセージ付きコンストラクタ
   - ✅ メッセージと内部例外付きコンストラクタ

2. **ApiBadRequestException**
   - ✅ メッセージ付きコンストラクタ

3. **ApiNotFoundException**
   - ✅ メッセージ付きコンストラクタ

## 4. テスト実装のベストプラクティス

### 4.1 テストメソッドの命名規則

以下の命名規則に従います：

```
[メソッド名]_[シナリオ]_[期待される結果]
```

例:
- `Get_正常系`
- `Get_ById_対象無し`
- `Add_descriptionがnull_ApiBadRequestExceptionが発生`

### 4.2 テストの構造

すべてのテストは **Arrange-Act-Assert (AAA) パターン** に従います：

```csharp
[TestMethod]
public void MethodName_Scenario_ExpectedResult()
{
    // Arrange: テストデータとモックの準備
    var mock = new Mock<IRepository<Todo>>();
    var service = new TodoService(mock.Object);
    
    // Act: テスト対象のメソッドを実行
    var result = service.Get(1);
    
    // Assert: 結果を検証
    Assert.IsNotNull(result);
    Assert.AreEqual(1, result.Id);
}
```

### 4.3 モックの使用

- **依存関係の分離**: 外部依存（リポジトリ、サービスなど）は必ずモック化
- **Moq ライブラリ**: 既存のテストで使用されているMoqを継続使用
- **検証**: メソッドが呼び出されたことを検証する場合は `Verify` を使用

### 4.4 統合テストの実装

統合テストでは以下の点に注意：

- **IntegrationTestBase**: 既存の `IntegrationTestBase` を継承
- **TestServer**: `Microsoft.Owin.Testing.TestServer` を使用
- **HTTPクライアント**: `HttpClient` を使用して実際のHTTPリクエストを送信
- **データのクリーンアップ**: テスト間でデータが影響しないようにする

### 4.5 例外テスト

例外が発生することをテストする場合：

```csharp
[TestMethod]
[ExpectedException(typeof(ApiNotFoundException))]
public void Get_ById_対象無し()
{
    // Arrange
    var mock = new Mock<IRepository<Todo>>();
    mock.Setup(_ => _.Get()).Returns(new List<Todo>());
    var service = new TodoService(mock.Object);
    
    // Act & Assert
    service.Get(999);
}
```

または、MSTest v2の `Assert.ThrowsException` を使用：

```csharp
[TestMethod]
public void Get_ById_対象無し()
{
    // Arrange
    var mock = new Mock<IRepository<Todo>>();
    mock.Setup(_ => _.Get()).Returns(new List<Todo>());
    var service = new TodoService(mock.Object);
    
    // Act & Assert
    Assert.ThrowsException<ApiNotFoundException>(() => service.Get(999));
}
```

### 4.6 データ駆動テスト

同じテストロジックを複数のデータで実行する場合、`DataTestMethod` と `DataRow` を使用：

```csharp
[DataTestMethod]
[DataRow(1)]
[DataRow(2)]
[DataRow(3)]
public void Get_ById_正常系(int id)
{
    // テスト実装
}
```

### 4.7 テストカテゴリの使用

テストカテゴリを使用してテストを分類：

- `[TestCategory("Logic")]`: ユニットテスト
- `[TestCategory("Integration")]`: 統合テスト
- `[TestCategory("Todo")]`: Todo機能関連
- `[TestCategory("Greeting")]`: Greeting機能関連

## 5. 実装の優先順位

### 優先度: 高

1. **TodoController** の不足テスト（Post, Put, Delete）
2. **GreetingController** の全テスト
3. **TodoService** の不足テスト（Add, Update, Remove）
4. **GreetingService** の Greeting(int hour) テスト
5. **ErrorHandlingMiddleware** の詳細テスト

### 優先度: 中

6. **TodoRepository** の全テスト
7. **ServiceProviderControllerActivator** のテスト

### 優先度: 低

8. **PassthroughExceptionHandler** のテスト
9. **ServiceProviderExtensions** のテスト
10. **Exceptions** のテスト（例外クラス自体がシンプルなため）

## 6. テスト実行とカバレッジ確認

### 6.1 テストの実行

```bash
# OpenCover.bat を使用してカバレッジを測定
.\OpenCover.bat
```

### 6.2 カバレッジレポートの確認

カバレッジレポートは `.\report` ディレクトリに生成されます。HTMLレポートを開いて、カバレッジの詳細を確認してください。

### 6.3 カバレッジの目標値

- **行カバレッジ**: 80%以上
- **分岐カバレッジ**: 75%以上
- **メソッドカバレッジ**: 90%以上

## 7. 実装例

### 7.1 TodoController の統合テスト例

```csharp
[TestMethod]
public async Task Post_正常系()
{
    // Arrange
    var todo = new ToDoPostRequest { Id = 1, Description = "Updated Todo" };
    var content = new StringContent(
        JsonConvert.SerializeObject(todo),
        Encoding.UTF8,
        "application/json");

    // Act
    var response = await HttpClient.PostAsync(_url, content);

    // Assert
    Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    var result = await response.Content.ReadAsAsync<Todo>();
    Assert.AreEqual(todo.Id, result.Id);
    Assert.AreEqual(todo.Description, result.Description);
}

[TestMethod]
public async Task Post_存在しないID_ApiNotFoundException()
{
    // Arrange
    var todo = new ToDoPostRequest { Id = 999, Description = "Not Found Todo" };
    var content = new StringContent(
        JsonConvert.SerializeObject(todo),
        Encoding.UTF8,
        "application/json");

    // Act
    var response = await HttpClient.PostAsync(_url, content);

    // Assert
    Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
    var result = await response.Content.ReadAsStringAsync();
    var error = JsonConvert.DeserializeObject<Dictionary<string, string>>(result);
    Assert.IsTrue(error.ContainsKey("error"));
}

[TestMethod]
public async Task Put_正常系()
{
    // Arrange
    var todo = new ToDoPutRequest { Description = "New Todo" };
    var content = new StringContent(
        JsonConvert.SerializeObject(todo),
        Encoding.UTF8,
        "application/json");

    // Act
    var response = await HttpClient.PutAsync(_url, content);

    // Assert
    Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    var result = await response.Content.ReadAsAsync<Todo>();
    Assert.IsNotNull(result);
    Assert.AreEqual(todo.Description, result.Description);
}

[TestMethod]
public async Task Delete_正常系()
{
    // Arrange
    var id = 1;

    // Act
    var response = await HttpClient.DeleteAsync(_url + $"?id={id}");

    // Assert
    Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
}
```

### 7.2 GreetingController の統合テスト例

```csharp
[TestClass]
[TestCategory("Greeting"), TestCategory("Integration")]
public class GreetingControllerTest : IntegrationTestBase
{
    private readonly string _url = "http://localhost/api/greeting";

    [ClassInitialize]
    public static void Setup(TestContext context)
    {
        Before();
    }

    [ClassCleanup]
    public static void TearDown()
    {
        After();
    }

    [TestMethod]
    public async Task Get_正常系()
    {
        // Arrange・Act
        var response = await HttpClient.GetAsync(_url);

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadAsAsync<string>();
        Assert.AreEqual("Hello", result);
    }

    [DataTestMethod]
    [DataRow(0, "Good evening")]
    [DataRow(4, "Good evening")]
    [DataRow(5, "Good morning")]
    [DataRow(11, "Good morning")]
    [DataRow(12, "Good afternoon")]
    [DataRow(17, "Good afternoon")]
    [DataRow(18, "Good evening")]
    [DataRow(23, "Good evening")]
    public async Task Get_by_hour_正常系(int hour, string expected)
    {
        // Arrange・Act
        var response = await HttpClient.GetAsync(_url + $"/{hour}");

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadAsAsync<string>();
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public async Task Get_by_hour_24以上_ApiBadRequestException()
    {
        // Arrange
        var hour = 24;

        // Act
        var response = await HttpClient.GetAsync(_url + $"/{hour}");

        // Assert
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        var result = await response.Content.ReadAsStringAsync();
        var error = JsonConvert.DeserializeObject<Dictionary<string, string>>(result);
        Assert.IsTrue(error.ContainsKey("error"));
    }
}
```

### 7.3 TodoService のユニットテスト例

```csharp
[TestMethod]
public void Add_正常系()
{
    // Arrange
    _mock.Setup(_ => _.Get())
        .Returns(_data);
    _service = new TodoService(_mock.Object);
    var description = "New Todo Item";

    // Act
    var result = _service.Add(description);

    // Assert
    Assert.IsNotNull(result);
    Assert.AreEqual(5, result.Id); // Max ID + 1
    Assert.AreEqual(description, result.Description);
    _mock.Verify(_ => _.Add(It.IsAny<Todo>()), Times.Once);
}

[TestMethod]
[ExpectedException(typeof(ApiBadRequestException))]
public void Add_descriptionがnull_ApiBadRequestExceptionが発生()
{
    // Arrange
    _mock.Setup(_ => _.Get())
        .Returns(_data);
    _service = new TodoService(_mock.Object);

    // Act & Assert
    _service.Add(null);
}

[TestMethod]
public void Update_正常系()
{
    // Arrange
    _mock.Setup(_ => _.Get())
        .Returns(_data);
    _service = new TodoService(_mock.Object);
    var id = 1;
    var description = "Updated Description";

    // Act
    _service.Update(id, description);

    // Assert
    var result = _service.Get(id);
    Assert.AreEqual(description, result.Description);
    _mock.Verify(_ => _.Update(It.IsAny<Todo>()), Times.Once);
}

[TestMethod]
[ExpectedException(typeof(ApiNotFoundException))]
public void Update_存在しないID_ApiNotFoundExceptionが発生()
{
    // Arrange
    _mock.Setup(_ => _.Get())
        .Returns(_data);
    _service = new TodoService(_mock.Object);

    // Act & Assert
    _service.Update(999, "Description");
}

[TestMethod]
public void Remove_正常系()
{
    // Arrange
    _mock.Setup(_ => _.Get())
        .Returns(_data);
    _service = new TodoService(_mock.Object);
    var id = 1;

    // Act
    _service.Remove(id);

    // Assert
    _mock.Verify(_ => _.Remove(It.IsAny<Todo>()), Times.Once);
}

[TestMethod]
[ExpectedException(typeof(ApiNotFoundException))]
public void Remove_存在しないID_ApiNotFoundExceptionが発生()
{
    // Arrange
    _mock.Setup(_ => _.Get())
        .Returns(_data);
    _service = new TodoService(_mock.Object);

    // Act & Assert
    _service.Remove(999);
}
```

### 7.4 GreetingService のユニットテスト例

```csharp
[DataTestMethod]
[DataRow(0, "Good evening")]
[DataRow(4, "Good evening")]
[DataRow(5, "Good morning")]
[DataRow(11, "Good morning")]
[DataRow(12, "Good afternoon")]
[DataRow(17, "Good afternoon")]
[DataRow(18, "Good evening")]
[DataRow(23, "Good evening")]
public void Greeting_by_hour_正常系(int hour, string expected)
{
    // Arrange・Act
    var result = _service.Greeting(hour);

    // Assert
    Assert.AreEqual(expected, result);
}

[TestMethod]
[ExpectedException(typeof(ApiBadRequestException))]
public void Greeting_by_hour_24以上_ApiBadRequestExceptionが発生()
{
    // Arrange・Act & Assert
    _service.Greeting(24);
}
```

### 7.5 TodoRepository のユニットテスト例

```csharp
[TestClass]
[TestCategory("Todo"), TestCategory("Logic")]
public class TodoRepositoryTest
{
    private TodoRepository _repository;

    [TestInitialize]
    public void Before()
    {
        _repository = new TodoRepository();
    }

    [TestMethod]
    public void Get_正常系()
    {
        // Arrange・Act
        var result = _repository.Get().ToList();

        // Assert
        Assert.IsTrue(result.Any());
        Assert.IsTrue(result.Any(_ => _.Id == 0));
    }

    [TestMethod]
    public void Add_正常系()
    {
        // Arrange
        var todo = new Todo { Id = 100, Description = "New Todo", CreatedDate = DateTime.Now };

        // Act
        _repository.Add(todo);

        // Assert
        var result = _repository.Get().FirstOrDefault(_ => _.Id == 100);
        Assert.IsNotNull(result);
        Assert.AreEqual(todo.Description, result.Description);
    }

    [TestMethod]
    public void Update_正常系()
    {
        // Arrange
        var todo = new Todo { Id = 1, Description = "Updated Todo", CreatedDate = DateTime.Now };

        // Act
        _repository.Update(todo);

        // Assert
        var result = _repository.Get().FirstOrDefault(_ => _.Id == 1);
        Assert.IsNotNull(result);
        Assert.AreEqual(todo.Description, result.Description);
    }

    [TestMethod]
    public void Remove_正常系()
    {
        // Arrange
        var todo = _repository.Get().First();

        // Act
        _repository.Remove(todo);

        // Assert
        var result = _repository.Get().FirstOrDefault(_ => _.Id == todo.Id);
        Assert.IsNull(result);
    }
}
```

### 7.6 ErrorHandlingMiddleware のテスト例

```csharp
[TestMethod]
public async Task HandleException_ApiBadRequestException_HTTPステータス400()
{
    // Arrange
    var server = TestServer.Create<TestStartupWithBadRequestException>();
    var client = new HttpClient(server.Handler);

    // Act
    var response = await client.GetAsync("http://localhost/api/test");

    // Assert
    Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
    var json = await response.Content.ReadAsStringAsync();
    var result = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
    Assert.AreEqual("Bad Request Error", result["error"]);

    client.Dispose();
    server.Dispose();
}

[TestMethod]
public async Task HandleException_ApiNotFoundException_HTTPステータス404()
{
    // Arrange
    var server = TestServer.Create<TestStartupWithNotFoundException>();
    var client = new HttpClient(server.Handler);

    // Act
    var response = await client.GetAsync("http://localhost/api/test");

    // Assert
    Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
    var json = await response.Content.ReadAsStringAsync();
    var result = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
    Assert.AreEqual("Not Found Error", result["error"]);

    client.Dispose();
    server.Dispose();
}
```

## 8. まとめ

本設計指針に従ってテストケースを追加することで、以下の効果が期待できます：

1. **コード品質の向上**: バグの早期発見
2. **リファクタリングの安全性**: 既存機能が壊れていないことを確認
3. **ドキュメントとしての役割**: テストコードが仕様書として機能
4. **保守性の向上**: 変更時の影響範囲を把握

段階的にテストケースを追加し、カバレッジ率を向上させていきましょう。

### 8.1 次のステップ

1. **優先度の高いテストから実装**: Controllers と Services の不足テストを最初に実装
2. **カバレッジの測定**: 各実装後に OpenCover.bat を実行してカバレッジを確認
3. **継続的な改善**: カバレッジレポートを確認し、不足している部分を特定して追加
4. **コードレビュー**: テストコードも本番コードと同様にレビューを行う

