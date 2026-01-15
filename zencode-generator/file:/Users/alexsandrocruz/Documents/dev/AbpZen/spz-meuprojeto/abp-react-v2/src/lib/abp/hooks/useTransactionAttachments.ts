import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetTransactionAttachmentsInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  transactionId?: string;
  }

export function useTransactionAttachments(input: GetTransactionAttachmentsInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["transactionAttachments", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/transaction-attachment", {
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

export function useAllTransactionAttachments() {
  return useQuery({
    queryKey: ["transactionAttachments", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/transaction-attachment", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useTransactionAttachment(id: string) {
  return useQuery({
    queryKey: ["transactionAttachment", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/transaction-attachment/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateTransactionAttachment() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/transaction-attachment", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["transactionAttachments"] });
    },
  });
}

export function useUpdateTransactionAttachment() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/transaction-attachment/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["transactionAttachments"] });
      queryClient.invalidateQueries({ queryKey: ["transactionAttachment", data.id] });
    },
  });
}

export function useDeleteTransactionAttachment() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/transaction-attachment/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["transactionAttachments"] });
    },
  });
}


