using System;
using System.Collections;
using System.Linq;
using System.Web.UI;

public abstract class ItemsData : IHierarchicalEnumerable, IEnumerable {
	IEnumerable data;

	public ItemsData() {
		this.data = GetData();
	}

	public IEnumerator GetEnumerator() {
		return this.data.GetEnumerator();
	}
	public IHierarchyData GetHierarchyData(object enumeratedItem) {
		return (IHierarchyData)enumeratedItem;
	}

	public abstract IEnumerable GetData();
}

public class ItemData : IHierarchyData {
	public string Text { get; protected set; }
	public string NavigateUrl { get; protected set; }

	public ItemData(string text, string navigateUrl) {
		Text = text;
		NavigateUrl = navigateUrl;
	}

	// IHierarchyData
	bool IHierarchyData.HasChildren {
		get { return HasChildren(); }
	}
	object IHierarchyData.Item {
		get { return this; }
	}
	string IHierarchyData.Path {
		get { return NavigateUrl; }
	}
	string IHierarchyData.Type {
		get { return GetType().ToString(); }
	}
	IHierarchicalEnumerable IHierarchyData.GetChildren() {
		return CreateChildren();
	}
	IHierarchyData IHierarchyData.GetParent() {
		return null;
	}

	protected virtual bool HasChildren() {
		return false;
	}
	protected virtual IHierarchicalEnumerable CreateChildren() {
		return null;
	}
}


public class CategoriesData : ItemsData {
	public override IEnumerable GetData() {
		return from category in NorthwindDataProvider.DB.Categories select new CategoryData(category);
	}
}

public class CategoryData : ItemData {
	public Category Category { get; protected set; }

	public CategoryData(Category category)
		: base(category.CategoryName, "?CategoryID=" + category.CategoryID) {
		Category = category;
	}

	protected override bool HasChildren() {
		return true;
	}
	protected override IHierarchicalEnumerable CreateChildren() {
		return new ProductsData(Category.CategoryID);
	}
}

public class ProductsData : ItemsData {
	public int CategoryID { get; protected set; }

	public ProductsData(int categoryID)
		: base() {
		CategoryID = categoryID;
	}

	public override IEnumerable GetData() {
		return from product in NorthwindDataProvider.DB.Products where product.CategoryID == CategoryID select new ProductData(product);
	}
}

public class ProductData : ItemData {
	public ProductData(Product product)
		: base(product.ProductName, "?CategoryID=" + product.CategoryID + "&ProductID=" + product.ProductID) {
	}
}