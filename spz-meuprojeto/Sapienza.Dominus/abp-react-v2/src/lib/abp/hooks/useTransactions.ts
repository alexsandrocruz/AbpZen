import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetTransactionsInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  clientId?: string;
  financialCategoryId?: string;
  }

export function useTransactions(input: GetTransactionsInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["transactions", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/transaction", {
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

export function useAllTransactions() {
  return useQuery({
    queryKey: ["transactions", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/transaction", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useTransaction(id: string) {
  return useQuery({
    queryKey: ["transaction", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/transaction/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateTransaction() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/transaction", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["transactions"] });
    },
  });
}

export function useUpdateTransaction() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/transaction/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["transactions"] });
      queryClient.invalidateQueries({ queryKey: ["transaction", data.id] });
    },
  });
}

export function useDeleteTransaction() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/transaction/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["transactions"] });
    },
  });
}





/**
 * Toggle hook for many-to-many relationship: Client <-> FinancialCategory
 * Given a Client, toggle a FinancialCategory
 */
export function useToggleFinancialCategory(clientId: string) {
  const queryClient = useQueryClient();
  const createMutation = useCreateTransaction();
  const deleteMutation = useDeleteTransaction();
  const { data: existing } = useTransactions({ clientId: clientId, maxResultCount: 1000 });

  return useMutation({
    mutationFn: async ({ financialCategoryId, isChecked }: { financialCategoryId: string; isChecked: boolean }) => {
      if (isChecked) {
        // Remove relationship
        const record = existing?.items?.find((i: any) => i.financialCategoryId === financialCategoryId);
        if (record) {
          await deleteMutation.mutateAsync(record.id);
        }
      } else {
        // Add relationship
        await createMutation.mutateAsync({
          clientId: clientId,
          financialCategoryId: financialCategoryId,
        });
      }
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["transactions"] });
    },
  });
}

/**
 * Toggle hook for many-to-many relationship: Client <-> FinancialCategory
 * Given a FinancialCategory, toggle a Client
 */
export function useToggleClient(financialCategoryId: string) {
  const queryClient = useQueryClient();
  const createMutation = useCreateTransaction();
  const deleteMutation = useDeleteTransaction();
  const { data: existing } = useTransactions({ financialCategoryId: financialCategoryId, maxResultCount: 1000 });

  return useMutation({
    mutationFn: async ({ clientId, isChecked }: { clientId: string; isChecked: boolean }) => {
      if (isChecked) {
        // Remove relationship
        const record = existing?.items?.find((i: any) => i.clientId === clientId);
        if (record) {
          await deleteMutation.mutateAsync(record.id);
        }
      } else {
        // Add relationship
        await createMutation.mutateAsync({
          clientId: clientId,
          financialCategoryId: financialCategoryId,
        });
      }
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["transactions"] });
    },
  });
}

