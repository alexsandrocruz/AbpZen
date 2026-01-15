import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetFinancialCategoriesInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useFinancialCategories(input: GetFinancialCategoriesInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["financialCategories", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/financial-category", {
        params: {
          filter,
          skipCount,
          maxResultCount,
          ...rest,
        },
      });
      return response.data;
    },
  });
}

export function useAllFinancialCategories() {
  return useQuery({
    queryKey: ["financialCategories", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/financial-category", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useFinancialCategory(id: string) {
  return useQuery({
    queryKey: ["financialCategory", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/financial-category/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateFinancialCategory() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/financial-category", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["financialCategories"] });
    },
  });
}

export function useUpdateFinancialCategory() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/financial-category/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["financialCategories"] });
      queryClient.invalidateQueries({ queryKey: ["financialCategory", data.id] });
    },
  });
}

export function useDeleteFinancialCategory() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/financial-category/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["financialCategories"] });
    },
  });
}


