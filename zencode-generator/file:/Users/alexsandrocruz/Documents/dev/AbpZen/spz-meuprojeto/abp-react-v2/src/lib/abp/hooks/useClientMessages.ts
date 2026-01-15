import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetClientMessagesInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  clientId?: string;
  }

export function useClientMessages(input: GetClientMessagesInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["clientMessages", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/client-message", {
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

export function useAllClientMessages() {
  return useQuery({
    queryKey: ["clientMessages", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/client-message", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useClientMessage(id: string) {
  return useQuery({
    queryKey: ["clientMessage", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/client-message/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateClientMessage() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/client-message", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["clientMessages"] });
    },
  });
}

export function useUpdateClientMessage() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/client-message/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["clientMessages"] });
      queryClient.invalidateQueries({ queryKey: ["clientMessage", data.id] });
    },
  });
}

export function useDeleteClientMessage() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/client-message/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["clientMessages"] });
    },
  });
}


