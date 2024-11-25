// See https://aka.ms/new-console-template for more information

// Definição básica de nós de expressões lógicas
abstract class Expr { }

class ResultExpr : Expr
{
    public Expr Result { get; }

    public ResultExpr(Expr result)
    {
        Result = result;
    }

    public string RemoveParentesesOnBoarders(string str)
    {
        if (str.Length > 1 && str[0] == '(' && str[str.Length - 1] == ')')
        {
            return str.Substring(1, str.Length - 2);
        }
        return str;
    }

    public override string ToString()
    {
        return RemoveParentesesOnBoarders(Result?.ToString() ?? string.Empty);
    }
}

class Literal : Expr
{
    public string Name { get; }
    public bool Negated { get; }

    public Literal(string name, bool negated = false)
    {
        Name = name;
        Negated = negated;
    }

    public override string ToString()
    {
        return Negated ? $"¬{Name}" : Name;
    }
}

class And : Expr
{
    public Expr Left { get; }
    public Expr Right { get; }

    public And(Expr left, Expr right)
    {
        Left = left;
        Right = right;
    }

    public override string ToString()
    {
        return $"({Left} ∧ {Right})";
    }
}

class Or : Expr
{
    public Expr Left { get; }
    public Expr Right { get; }

    public Or(Expr left, Expr right)
    {
        Left = left;
        Right = right;
    }

    public override string ToString()
    {
        return $"({Left} ∨ {Right})";
    }
}

class Not : Expr
{
    public Expr Expression { get; }

    public Not(Expr expression)
    {
        Expression = expression;
    }

    public override string ToString()
    {
        return $"¬({Expression})";
    }
}

class Implication : Expr
{
    public Expr Left { get; }
    public Expr Right { get; }

    public Implication(Expr left, Expr right)
    {
        Left = left;
        Right = right;
    }

    public override string ToString()
    {
        return $"({Left} → {Right})";
    }
}

class Biconditional : Expr
{
    public Expr Left { get; }
    public Expr Right { get; }

    public Biconditional(Expr left, Expr right)
    {
        Left = left;
        Right = right;
    }

    public override string ToString()
    {
        return $"({Left} ↔ {Right})";
    }
}

class FNDConverter
{
    // Eliminar bicondicionais (<=>)
    public Expr EliminarBicondicionais(Expr expr)
    {
        if (expr is Biconditional biconditional)
        {
            var a = biconditional.Left;
            var b = biconditional.Right;
            return new And(new Or(new Not(a), b), new Or(new Not(b), a));
        }
        return expr;
    }

    // Eliminar implicações(->)
    public Expr EliminarImplicacoes(Expr expr)
    {
        if (expr is Implication implication)
        {
            var a = implication.Left;
            var b = implication.Right;
            return new Or(new Not(a), b);
        }
        return expr;
    }

    // Mover negações(¬) para os literais
    public Expr MoverNegacoes(Expr expr)
    {
        if (expr is Not notExpr)
        {
            var subExpr = notExpr.Expression;
            if (subExpr is And andExpr)
            {
                // ¬(A ∧ B) -> ¬A ∨ ¬B
                return new Or(MoverNegacoes(new Not(andExpr.Left)), MoverNegacoes(new Not(andExpr.Right)));
            }
            else if (subExpr is Or orExpr)
            {
                // ¬(A ∨ B) -> ¬A ∧ ¬B
                return new And(MoverNegacoes(new Not(orExpr.Left)), MoverNegacoes(new Not(orExpr.Right)));
            }
            else if (subExpr is Not nestedNotExpr)
            {
                // ¬¬A -> A
                return MoverNegacoes(nestedNotExpr.Expression);
            }
        }
        else if (expr is And andExpr)
        {
            return new And(MoverNegacoes(andExpr.Left), MoverNegacoes(andExpr.Right));
        }
        else if (expr is Or orExpr)
        {
            return new Or(MoverNegacoes(orExpr.Left), MoverNegacoes(orExpr.Right));
        }
        return expr;
    }

    // Aplicar a distributividade
    public Expr AplicarDistributividade(Expr expr)
    {
        if (expr is Or orExpr)
        {
            if (orExpr.Left is And leftAndExpr)
            {
                // A ∨ (B ∧ C) -> (A ∨ B) ∧ (A ∨ C)
                return new And(AplicarDistributividade(new Or(leftAndExpr.Left, orExpr.Right)),
                               AplicarDistributividade(new Or(leftAndExpr.Right, orExpr.Right)));
            }
            else if (orExpr.Right is And rightAndExpr)
            {
                // (A ∧ B) ∨ C -> (A ∨ C) ∧ (B ∨ C)
                return new And(AplicarDistributividade(new Or(orExpr.Left, rightAndExpr.Left)),
                               AplicarDistributividade(new Or(orExpr.Left, rightAndExpr.Right)));
            }
        }
        else if (expr is And andExpr)
        {
            return new And(AplicarDistributividade(andExpr.Left), AplicarDistributividade(andExpr.Right));
        }
        return expr;
    }

    // Função principal para converter em FND
    public Expr ConverterParaFND(Expr expr)
    {
        expr = EliminarBicondicionais(expr);
        expr = EliminarImplicacoes(expr);
        expr = MoverNegacoes(expr);
        expr = AplicarDistributividade(expr);
        return new ResultExpr(expr);
    }
}

class Program
{
    static void Main(string[] args)
    {
        var expr = CreateExpr();
        var converter = new FNDConverter();
        var fnd = converter.ConverterParaFND(expr);

        // Configura o console para usar UTF-8
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine($"--------------------------------------------------");
        Console.WriteLine($"Sentença original: {expr}");
        Console.WriteLine($"Sentença em FND: {fnd}");
    }

    private static Expr CreateExpr()
    {
        var a = new Literal("A");
        var b = new Literal("B");
        var c = new Literal("C");
        var d = new Literal("D");

        // Exemplo: ¬(A → B) ∨ (C ∧ ¬D)
        //var expr = new Or(new Not(new Implication(a, b)), new And(c, new Not(d)));

        /* Questao 3c.
            1. ¬A ∨ B
            2. ¬B ∨ C
            3. ¬C ∨ ¬A
        */

        // Exemplo: A → B
        var expr = new Implication(a, b);
        // Exemplo: B ⇒ C
        var expr2 = new Implication(b, c);
        // Exemplo: C ⇒ ¬A
        var expr3 = new Implication(c, new Not(a));

        return expr3;
    }
}

