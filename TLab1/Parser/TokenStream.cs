using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TLab1.Parser
{
    public class TokenStream
    {
        private readonly List<Token> _tokens;
        private readonly Token _fallbackToken;
        private int _position;

        public TokenStream(List<Token> tokens)
        {
            _tokens = tokens;
            _position = 0;
            _fallbackToken = new Token
            {
                Type = 0,
                Value = string.Empty,
                Line = 0,
                StartPos = 0,
                EndPos = 0,
                AbsoluteIndex = 0
            };
        }

        public Token Current
        {
            get
            {
                if (_tokens.Count == 0)
                    return _fallbackToken;

                if (IsAtEnd)
                    return _tokens[_tokens.Count - 1];

                return _tokens[_position];
            }
        }

        public Token Previous
        {
            get
            {
                if (_tokens.Count == 0)
                    return _fallbackToken;

                if (_position <= 0)
                    return _tokens[0];

                return _tokens[_position - 1];
            }
        }

        public bool IsAtEnd
        {
            get { return _position >= _tokens.Count; }
        }

        public int Position
        {
            get { return _position; }
        }

        public void Advance()
        {
            if (!IsAtEnd)
                _position++;
        }

        public bool Check(TokenType code)
        {
            return !IsAtEnd && Current.Code == (int)code;
        }

        public bool Match(TokenType code)
        {
            if (!Check(code))
                return false;

            Advance();
            return true;
        }
    }
}
