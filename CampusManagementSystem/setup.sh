#!/bin/bash

# Build Campus Management System

echo ""
echo "======================================"
echo "Campus Management System - Setup"
echo "======================================"
echo ""

# Check if .NET SDK is installed
if ! command -v dotnet &> /dev/null; then
    echo "ERROR: .NET SDK is not installed. Please install .NET 8 SDK first."
    exit 1
fi

# Check if Node.js is installed
if ! command -v node &> /dev/null; then
    echo "ERROR: Node.js is not installed. Please install Node.js first."
    exit 1
fi

echo "[1/4] Restoring backend dependencies..."
cd backend
dotnet restore
if [ $? -ne 0 ]; then
    echo "ERROR: Failed to restore backend dependencies"
    cd ..
    exit 1
fi
cd ..

echo ""
echo "[2/4] Building backend..."
cd backend
dotnet build
if [ $? -ne 0 ]; then
    echo "ERROR: Failed to build backend"
    cd ..
    exit 1
fi
cd ..

echo ""
echo "[3/4] Installing frontend dependencies..."
cd frontend
npm install
if [ $? -ne 0 ]; then
    echo "ERROR: Failed to install frontend dependencies"
    cd ..
    exit 1
fi
cd ..

echo ""
echo "[4/4] Building frontend..."
cd frontend
npm run build
if [ $? -ne 0 ]; then
    echo "ERROR: Failed to build frontend"
    cd ..
    exit 1
fi
cd ..

echo ""
echo "======================================"
echo "Setup completed successfully!"
echo "======================================"
echo ""
echo "Next steps:"
echo "1. Ensure SQL Server is running"
echo "2. Update backend/appsettings.json with your connection string"
echo "3. Run migrations: dotnet ef database update"
echo "4. Start backend: dotnet run"
echo "5. Start frontend: npm run dev"
echo ""
