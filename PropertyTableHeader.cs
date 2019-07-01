namespace CustomerTestsExcel
{
    public class PropertyTableHeader : TableHeader, ITableHeader
    {
        public PropertyTableHeader(string propertyName) : base(propertyName) { }

        public bool Equals(ITableHeader other)
        {
            return (GetType().Equals(other.GetType()) && (other as PropertyTableHeader).PropertyName == PropertyName);
        }
    }
}
