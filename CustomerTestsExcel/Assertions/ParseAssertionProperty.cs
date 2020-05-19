using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace CustomerTestsExcel.Assertions
{
    public class ParseAssertionProperty : ExpressionVisitor
    {
        protected readonly StringBuilder _propertyName;

        public string PropertyName => _propertyName.ToString();

        public string Index { get; }

        public ParseAssertionProperty(Expression exp)
        {
            _propertyName = new StringBuilder();

            Visit(exp);
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            base.VisitMember(node);

            _propertyName.Append(node.Member.Name); // could add the "." in some cases

            return node;
        }

        protected override Expression VisitMethodCall(MethodCallExpression m)
        {
            if (m.Method.Name == "get_Item")
            {
                _propertyName.Append("[" + m.Arguments[0].ToString() + "]");
            }
            else
            {
                _propertyName.Append(m.Method.Name);
                _propertyName.Append("(");
                AppendArgumentlist(m.Arguments);
                _propertyName.Append(")");
            }

            return m;
        }

        public void AppendArgumentlist(IEnumerable<Expression> arguments)
        {
            IEnumerator<Expression> enumerator = arguments.GetEnumerator();

            if (enumerator.MoveNext()) _propertyName.Append(enumerator.Current.ToString());

            while (enumerator.MoveNext())
            {
                _propertyName.Append(", ");
                _propertyName.Append(enumerator.Current.ToString());
            }
        }


        protected override Expression VisitUnary(UnaryExpression u)
        {
            return base.VisitUnary(u);
            //throw Unsupported(u);
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            return base.VisitParameter(node);
            //throw Unsupported(node);
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            throw Unsupported(node);
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            throw Unsupported(node);
        }

        protected override Expression VisitConditional(ConditionalExpression c)
        {
            throw Unsupported(c);
        }

        protected override Expression VisitInvocation(InvocationExpression iv)
        {
            throw Unsupported(iv);
        }

        protected override Expression VisitListInit(ListInitExpression init)
        {
            throw Unsupported(init);
        }

        protected override MemberAssignment VisitMemberAssignment(MemberAssignment assignment)
        {
            throw Unsupported(assignment);
        }

        protected override Expression VisitMemberInit(MemberInitExpression init)
        {
            throw Unsupported(init);
        }

        protected override MemberListBinding VisitMemberListBinding(MemberListBinding binding)
        {
            throw Unsupported(binding);
        }

        protected override MemberMemberBinding VisitMemberMemberBinding(MemberMemberBinding binding)
        {
            throw Unsupported(binding);
        }

        protected override Expression VisitNewArray(NewArrayExpression na)
        {
            throw Unsupported(na);
        }

        protected Exception Unsupported(object node)
        {
            return new Exception(node.GetType().Name + " not supported");
        }

    }
}
